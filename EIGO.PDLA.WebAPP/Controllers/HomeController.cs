using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EIGO.PDLA.WebAPP.Controllers;

[Authorize(Policy = "AnyGroup")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DeclaracionesContext _declaracionesContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(IHttpContextAccessor accessor, ILogger<HomeController> logger, DeclaracionesContext declaracionesContext)
    {
        _logger = logger;
        _declaracionesContext = declaracionesContext;
        _httpContextAccessor = accessor;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
    }


    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ErrorDetalle(string error)
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier, ErrorMsg = error });
    }
}
