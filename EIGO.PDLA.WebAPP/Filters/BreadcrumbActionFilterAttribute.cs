using EIGO.PDLA.WebAPP.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace EIGO.PDLA.WebAPP.Filters;

public class BreadcrumbActionFilterAttribute : ActionFilterAttribute
{
    public static ICollection<Breadcrumb> ConfigureBreadcrumb(ActionExecutedContext context)
    {
        List<Breadcrumb> breadcrumbs = new();
        var path = context.HttpContext.Request.Path.Value?.ToString();
        var query = context.HttpContext.Request.Query;
        if (string.IsNullOrEmpty(path))
            return breadcrumbs;

        if (int.TryParse(path.Split('/').Last(), out _))
        {
            string[] temp = path.Split('/');
            Array.Resize(ref temp, temp.Length - 1);
            path = string.Join("/", temp);
        }

        var declaracionesFuncionarioSub = new Regex(@"^(/DeclaracionesFuncionario/){1}((create)|(edit)|(details)|(delete))", RegexOptions.IgnoreCase);

        var rootBc = new Breadcrumb
        {
            Action = "Index",
            Controller = "DeclaracionesFuncionario",
            Icon = "fa-home",
            IsActive = true,
            Text = "Inicio"
        };

        var declaracionesFuncionarioBc = new Breadcrumb
        {
            Action = "Index",
            Controller = "DeclaracionesFuncionario",
            IsActive = true,
            Text = "Declaraciones"
        };

        // Root
        breadcrumbs.Add(rootBc);

        if (declaracionesFuncionarioSub.IsMatch(path))
        {
            breadcrumbs.Add(declaracionesFuncionarioBc);
            var splitted = path.Split('/').Last();
            breadcrumbs.Add(new Breadcrumb
            {
                Action = "Index",
                Controller = "DeclaracionesFuncionarios",
                IsActive = false,
                Text = TextHelper.ReplaceText(splitted)
            });
        }

        breadcrumbs.Last().IsActive = false;
        return breadcrumbs;
    }
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Request.Path.HasValue && context.HttpContext.Request.Path.Value.Contains("api"))
        {
            // This is an API request.  
            base.OnActionExecuted(context);
            return;
        }

        var breadcrumbs = ConfigureBreadcrumb(context);

        var controller = context.Controller as Controller;
        controller.ViewBag.Breadcrumbs = breadcrumbs;

        base.OnActionExecuted(context);
    }
}
