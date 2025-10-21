using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class BasicAuthenticationAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var auth = context.HttpContext.Request.Headers.Authorization.FirstOrDefault(x => x.StartsWith("Basic"));
        if (auth == null)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "No authentication provided"
            };
            return;
        }
        var splitted = auth.Split(' ');
        if (splitted.Length != 2)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "No authentication provided"
            };
            return;
        }
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(splitted[1]));
        int separator = decoded.IndexOf(':');
        if (separator == -1)
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "No authentication provided"
            };
            return;
        }

        var username = decoded.Substring(0, separator);
        var password = decoded.Substring(separator + 1);

        var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        var apiUser = appSettings.GetValue<string>("API_USERNAME");
        var apiPass = appSettings.GetValue<string>("API_PASSWORD");

        if (!apiUser.Equals(username) || !apiPass.Equals(password))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Authentication not valid"
            };
            return;
        }

        await next();
    }
}
