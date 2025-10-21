using EIGO.PDLA.WebAPP.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [Area("administracion")]
    [Authorize(Policy = "GroupAdmin")]
    [BreadcrumbActionFilter]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
        
    }
}
