using EIGO.PDLA.Common.Exporter;
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;
[Area("Administracion")]
[Authorize(Policy = "GroupAdmin")]
public class ReportesController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IPdlaLogger _logger;

    public ReportesController(IConfiguration configuration,
        IPdlaLogger pdlaLogger)
    {
        _configuration = configuration;
        _logger = pdlaLogger;
    }

     public async Task<IActionResult> Index(string TipoReporte)
    {
        ReportesDTO reporte = new ReportesDTO
        {
            NombreReporte = ""
        }; 
        if (TipoReporte == "ReporteNegocio")
        {
             reporte = new ReportesDTO
            {
                NombreReporte = "Reporte de Negocio",
                Ruta = _configuration["Application:ReporteNegocio"]
            };

        }
        else if (TipoReporte == "Tecnologia")
        {
             reporte = new ReportesDTO
            {
                NombreReporte = "Reporte General de Auditoria",
                Ruta = _configuration["Application:AuditoriaProceso"]
            };

        }
        else if(TipoReporte == "Negocio")
        {
             reporte = new ReportesDTO
            {
                NombreReporte = "Reporte General de Auditoria",
                Ruta = _configuration["Application:AuditoriaProceso"]
            };
        }
        else
        {

        }
        

        return View(reporte);
    }
}
