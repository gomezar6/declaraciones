using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EIGO.PDLA.WebAPP.Helpers;

public static class BreadcrumbHelper
{
    public static List<Breadcrumb> ConfigureBreadcrumb(ActionExecutedContext context)
    {
        var breadcrumbList = new List<Breadcrumb>();
        var homeControllerName = "DeclaracionesFuncionario";

        breadcrumbList.Add(new Breadcrumb
        {
            Text = "Inicio",
            Action = "Index",
            Controller = homeControllerName, // Change this controller name to match your Home Controller.
            IsActive = true
        });

        if (context.HttpContext.Request.Path.HasValue)
        {
            var pathSplit = context.HttpContext.Request.Path.Value.Split("/");

            for (var i = 0; i < pathSplit.Length; i++)
            {
                TextInfo textInfo = new CultureInfo("es-ES", false).TextInfo;
                string value = textInfo.ToTitleCase(pathSplit[i]);
                // Check if first element is equal to our Index (home) page.
                if (string.IsNullOrEmpty(value) || string.Compare(value, homeControllerName, true) == 0)
                {
                    continue;
                }


                // First check if path is a Controller class.
                var controller = GetControllerType(value + "Controller");

                // If this is a controller, does it have a default Index method? If so, that needs adding as a breadcrumb. Is the next path element called Index?
                if (controller != null)
                {
                    var indexMethod = controller.GetMethod("Index");

                    if (indexMethod != null)
                    {
                        breadcrumbList.Add(new Breadcrumb
                        {
                            Text = value,
                            Action = "Index",
                            Controller = pathSplit[i],
                            IsActive = true
                        });

                        if (i + 1 < pathSplit.Length && string.Compare(pathSplit[i + 1], "Index", true) == 0)
                        {
                            // This is the last element in the breadcrumb list. This should be disabled.
                            breadcrumbList.LastOrDefault().IsActive = false;

                            // Next path item is the Index method. We can escape from this method now.
                            return breadcrumbList;
                        }
                    }
                }
                else
                {
                    breadcrumbList.Add(new Breadcrumb
                    {
                        Text = value,
                        Action = string.Empty,
                        Controller = string.Empty,
                        IsActive = false
                    });
                }

                // If not a Controller, check if this is a method on the previous path element.
                if (i - 1 > 0)
                {
                    var controllerName = pathSplit[i - 1] + "Controller";
                    var prevController = GetControllerType(controllerName);

                    if (prevController != null)
                    {
                        var method = prevController.GetMethod(pathSplit[i]);

                        if (method != null)
                        {
                            // We've found an endpoint on the previous controller.
                            breadcrumbList.Add(new Breadcrumb
                            {
                                Text = value,
                                Action = pathSplit[i],
                                Controller = pathSplit[i - 1]
                            });
                        }
                    }
                }
            }
        }

        // There will always be at least 1 entry in the breadcrumb list. The last element should always be disabled.
        breadcrumbList.LastOrDefault().IsActive = false;

        return breadcrumbList;
    }
    private static Type GetControllerType(string name)
    {
        Type controller = null;

        try
        {
            controller = Assembly.GetCallingAssembly().GetType("EIGO.PDLA.WebAPP.Controllers." + name);
            if (controller == null)
            {
                controller = Assembly.GetCallingAssembly().GetType("EIGO.PDLA.WebAPP.Areas.Administracion.Controllers." + name);
            }

        }
        catch
        {
            // Cannot do anything
        }

        return controller;
    }

    private static string CamelCaseSpacing(string s)
    {
        // Sourced from https://stackoverflow.com/questions/4488969/split-a-string-by-capital-letters.
        var r = new Regex(@"
        (?<=[A-Z])(?=[A-Z][a-z]) |
         (?<=[^A-Z])(?=[A-Z]) |
         (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        return r.Replace(s, " ");
    }

}
