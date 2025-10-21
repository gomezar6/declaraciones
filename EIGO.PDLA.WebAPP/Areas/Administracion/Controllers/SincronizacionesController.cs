using EIGO.PDLA.Common.Exporter;
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using EIGO.PDLA.WebAPP.Filters;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;
[Area("Administracion")]
[Authorize(Policy = "GroupAdmin")]
[TypeFilter(typeof(ErrorFilter))]
public class SincronizacionesController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IEntityRepository<Sincronizacion> _sincronizacionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPdlaLogger _logger;

    public SincronizacionesController(IConfiguration configuration,
        IEntityRepository<Sincronizacion> sincronizacionRepository,
        IHttpContextAccessor httpContextAccessor,
        IPdlaLogger pdlaLogger)
    {
        _configuration = configuration;
        _sincronizacionRepository = sincronizacionRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = pdlaLogger;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var sincronizaciones = await _sincronizacionRepository.GetAllActiveAsync();
        return View(sincronizaciones.ToSincronizacionDTOCollection().OrderByDescending(s => s.Fecha));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sincronizacion = await _sincronizacionRepository.GetByIdAsync(id.Value);
        sincronizacion.Fecha = sincronizacion.Fecha.ToLocalTime();

        if (sincronizacion == null)
        {
            return NotFound();
        }
        return View(sincronizacion.ToSincronizacionDTO());
    }

    public IActionResult Start()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartProcess()
    {
        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
        {
            HttpClient httpClient = new();
            try
            {
                if ((await _sincronizacionRepository.GetAllActiveAsync()).Any(s => s.EstatusSincronizacion.CompareTo("Iniciada") == 0))
                {
                    await _logger.LogAsync(null, "Inicio de sincronización", "Solicitud de inicio de sincronización", "Fallido: Existe otro proceso en ejecución", TipoAuditoria.Negocio);
                    return Json(new { succeed = false, status = 1 });
                }

                object user = new { user = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value };

                var response = await httpClient.PostAsync(
                    _configuration["Application:SyncServiceURL"],
                    new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json")
                    );

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogAsync(null, "Inicio de sincronización", "Solicitud de inicio de sincronización", "Exitoso", TipoAuditoria.Negocio);
                    return Json(new { succeed = true, status = 0 });
                }
                else
                {
                    await _logger.LogAsync(
                        null,
                        "Inicio de sincronización",
                        "Solicitud de inicio de sincronización",
                        $"Fallido: Status {response.StatusCode} {await response.Content.ReadAsStringAsync()}", TipoAuditoria.Negocio);
                    return Json(new { succeed = false, status = 2 });
                }
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(null, "Inicio de sincronización", "Solicitud de inicio de sincronización", $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                // TODO: Change to DI
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);
                return Json(new { succeed = false, status = 1 });
            }
        }

        await _logger.LogAsync(null, "Inicio de sincronización", "Solicitud de inicio de sincronización", "Fallido: sin respuesta válida", TipoAuditoria.Negocio);
        return Json(new { succeed = false, status = 2 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportDetails(int id)
    {
        var sincronizacion = await _sincronizacionRepository.GetByIdAsync(id);

        if (sincronizacion == null)
        {
            return NotFound();
        }

        Stream stream = Exporter.ExportToCSV(sincronizacion.SincronizacionDetalles.ToSincronizacionDetalleDTOCollection(),true);
        stream.Position = 0;
        return File(stream, "text/csv", $"ExportarDetallesSincronizacion-{id}.csv");
    }
}
