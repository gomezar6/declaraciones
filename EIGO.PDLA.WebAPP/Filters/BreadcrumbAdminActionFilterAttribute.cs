using EIGO.PDLA.WebAPP.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;

namespace EIGO.PDLA.WebAPP.Filters
{
    public class BreadcrumbAdminActionFilterAttribute : ActionFilterAttribute
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

            var adminRoot = new Regex(@"/administracion", RegexOptions.IgnoreCase);
            var adminProcesos = new Regex(@"/administracion/procesos", RegexOptions.IgnoreCase);
            var adminProcesosSub = new Regex(@"/administracion/procesos/", RegexOptions.IgnoreCase);
            var adminProcesosAlerta = new Regex(@"/administracion/alertas/", RegexOptions.IgnoreCase);
            var adminProcesosAlertasSub = new Regex(@"^(/administracion/alertas/){1}((create)|(edit)|(details)|(delete))", RegexOptions.IgnoreCase);
            var adminProcesosDisclaimers = new Regex(@"/administracion/disclaimers/", RegexOptions.IgnoreCase);
            var adminProcesosDisclaimersSub = new Regex(@"^(/administracion/disclaimers/){1}((create)|(edit)|(details)|(delete))", RegexOptions.IgnoreCase);
            var adminProcesosFormularios = new Regex(@"/administracion/formularios/", RegexOptions.IgnoreCase);
            var adminProcesosFormulariosSub = new Regex(@"^(/administracion/formularios/){1}((create)|(edit)|(details)|(delete))", RegexOptions.IgnoreCase);

            var rootBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Declaraciones",
                Icon = "fa-home",
                IsActive = true,
                Text = "Inicio"
            };

            var adminBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "declaraciones",
                IsActive = true,
                Text = "Administración"
            };

            var adminProcesosBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Procesos",
                IsActive = true,
                Text = "Procesos"
            };

            var adminAlertasBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Alertas",
                IsActive = false,
                Text = "Alertas"
            };

            var adminAlertasSubBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Alertas",
                IsActive = false,
                Text = "Alertas"
            };

            var adminDisclaimersBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Disclaimers",
                IsActive = false,
                Text = "Disclaimers"
            };

            var adminDisclaimersSubBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Disclaimers",
                IsActive = false,
                Text = "Disclaimers"
            };

            var adminFormulariosBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Formularios",
                IsActive = false,
                Text = "Formularios"
            };

            var adminFormulariosSubBc = new Breadcrumb
            {
                Action = "Index",
                Controller = "Formularios",
                IsActive = false,
                Text = "Formularios"
            };

            // Root
            breadcrumbs.Add(rootBc);

            if (adminRoot.IsMatch(path))
            {
                breadcrumbs.Add(adminBc);
            }

            if (adminProcesos.IsMatch(path))
            {
                breadcrumbs.Add(adminProcesosBc);
            }

            if (adminProcesosSub.IsMatch(path))
            {
                var splitted = path.Split('/').Last();
                adminAlertasBc.Text = TextHelper.ReplaceText(splitted);
                breadcrumbs.Add(adminAlertasBc);
            }

            if (adminProcesosAlerta.IsMatch(path))
            {
                var idProceso = query.ContainsKey("idProceso") ? query["idProceso"][0] : null;
                var origin = query.ContainsKey("origen") ? query["origen"][0] : null;
                if (!string.IsNullOrEmpty(idProceso) && !string.IsNullOrEmpty(origin))
                {
                    adminProcesosBc.Action = origin;
                    adminProcesosBc.Value = idProceso;
                }
                breadcrumbs.Add(adminProcesosBc);
                breadcrumbs.Add(adminAlertasBc);
            }

            if (adminProcesosAlertasSub.IsMatch(path))
            {
                var splitted = path.Split('/').Last();
                adminAlertasSubBc.Text = TextHelper.ReplaceText(splitted);
                breadcrumbs.Add(adminAlertasSubBc);
            }

            if (adminProcesosDisclaimers.IsMatch(path))
            {
                var idProceso = query.ContainsKey("idProceso") ? query["idProceso"][0] : null;
                var origin = query.ContainsKey("origen") ? query["origen"][0] : null;
                if (!string.IsNullOrEmpty(idProceso) && !string.IsNullOrEmpty(origin))
                {
                    adminProcesosBc.Action = origin;
                    adminProcesosBc.Value = idProceso;
                }
                breadcrumbs.Add(adminProcesosBc);
                breadcrumbs.Add(adminDisclaimersBc);
            }

            if (adminProcesosDisclaimersSub.IsMatch(path))
            {
                var splitted = path.Split('/').Last();
                adminDisclaimersSubBc.Text = TextHelper.ReplaceText(splitted);
                breadcrumbs.Add(adminDisclaimersSubBc);
            }

            if (adminProcesosFormularios.IsMatch(path))
            {
                var idProceso = query.ContainsKey("idProceso") ? query["idProceso"][0] : null;
                var origin = query.ContainsKey("origen") ? query["origen"][0] : null;
                if (!string.IsNullOrEmpty(idProceso) && !string.IsNullOrEmpty(origin))
                {
                    adminProcesosBc.Action = origin;
                    adminProcesosBc.Value = idProceso;
                }
                breadcrumbs.Add(adminProcesosBc);
                breadcrumbs.Add(adminFormulariosBc);
            }

            if (adminProcesosFormulariosSub.IsMatch(path))
            {
                var splitted = path.Split('/').Last();
                adminFormulariosSubBc.Text = TextHelper.ReplaceText(splitted);
                breadcrumbs.Add(adminFormulariosSubBc);
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
}
