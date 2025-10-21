using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EIGO.PDLA.WebAPP.Filters
{
    public class ErrorFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ErrorFilter(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
        


            var config = TelemetryConfiguration.CreateDefault();
            var client = new TelemetryClient(config);
            client.TrackException(context.Exception);
            context.Result = new RedirectResult("/Home/Error");




        }
    }

  
}
