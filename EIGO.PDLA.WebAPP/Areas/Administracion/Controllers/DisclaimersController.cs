#nullable disable
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using EIGO.PDLA.WebAPP.Filters;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Policy = "GroupAdmin")]
    [BreadcrumbAdminActionFilter]
    [TypeFilter(typeof(ErrorFilter))]
    public class DisclaimersController : Controller
    {
        private readonly IEntityRepository<Disclaimer> _disclaimersRepository;
        private readonly IEntityRepository<Proceso> _procesosRepository;
        private readonly IPdlaLogger _logger;
        public DisclaimersController(IEntityRepository<Disclaimer> disclaimersRepository, IEntityRepository<Proceso> procesosRepository, IPdlaLogger pdlaLogger)
        {
            _disclaimersRepository = disclaimersRepository;
            _procesosRepository = procesosRepository;
            _logger = pdlaLogger;
        }
        // GET: administracion/Disclaimers
        public async Task<IActionResult> Index()
        {
            return View(await _disclaimersRepository.GetAllActiveAsync());
        }
        // GET: administracion/Disclaimers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var disclaimer = await _disclaimersRepository.GetByIdAsync(id.Value);
            if (disclaimer == null)
            {
                return NotFound();
            }
            ViewData["SelectedValue"] = disclaimer.IdProceso;
            ViewBag.Editable = disclaimer.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(disclaimer);
        }
        // GET: administracion/Disclaimers/Create
        public IActionResult Create(int id)
        {
            ViewData["IdProceso"] = new SelectList(_procesosRepository.GetAllAsync().Result, "IdProceso", "NombreProceso",id);
            if (id == 0)
            {
                return NotFound();
            }
            ViewData["SelectedValue"] = id;
            return View();
        }
        // POST: administracion/Disclaimers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProceso,Titulo,Texto")] DisclaimerDTO disclaimer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                 //   await _logger.LogAsync(disclaimer.IdProceso, "Disclaimers:Crear", "Solicitud de inicio de creación de formulario", "Inicio de creación", TipoAuditoria.Negocio);
                    await _disclaimersRepository.AddAsync(disclaimer.ToDisclaimers());
                    await _logger.LogAsync(disclaimer.IdProceso, "Crear Disclaimers", disclaimer.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(disclaimer.IdProceso, "Crear Disclaimers", disclaimer.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);

                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                }

                return RedirectToAction("Edit", "Procesos", new { id = disclaimer.IdProceso, idTabs = "disclaimers" });
            }
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", disclaimer.IdProceso);
            ViewData["SelectedValue"] = disclaimer.IdProceso;
            return View(disclaimer);
        }
        // GET: administracion/Disclaimers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var disclaimer = await _disclaimersRepository.GetByIdAsync(id.Value);
            if (disclaimer == null)
            {
                return NotFound();
            }
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", disclaimer.IdProceso);
            ViewBag.Editable = disclaimer.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(disclaimer.ToDisclaimerDTO());
        }
        // POST: administracion/Disclaimers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDisclaimer,IdProceso,Titulo,Texto")] DisclaimerDTO disclaimer)
        {
            if (id != disclaimer.IdDisclaimer)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
            //        await _logger.LogAsync(disclaimer.IdProceso, "Disclaimers:Actualizar", "Solicitud de inicio de actualización de formulario", "Inicio de actualización", TipoAuditoria.Negocio);
                    await _disclaimersRepository.UpdateAsync(disclaimer.ToDisclaimers());
                    await _logger.LogAsync(disclaimer.IdProceso, "Actualizar Disclaimers", disclaimer.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(disclaimer.IdProceso, "Actualizar Disclaimers", disclaimer.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);

                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);

                    if (!await _disclaimersRepository.Exist(disclaimer.IdDisclaimer))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Procesos", new { id = disclaimer.IdProceso, idTabs = "disclaimers" });
            }
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", disclaimer.IdProceso);
            return View(disclaimer);
        }
        // GET: administracion/Disclaimers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var disclaimer = await _disclaimersRepository.GetByIdAsync(id.Value);
            if (disclaimer == null)
            {
                return NotFound();
            }
            ViewBag.Editable = disclaimer.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(disclaimer);
        }
        // POST: administracion/Disclaimers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disclaimer = await _disclaimersRepository.GetByIdAsync(id);
            if (disclaimer != null)
            {
                try
                {
          //          await _logger.LogAsync(disclaimer.IdProceso, "Disclaimers:Eliminar", "Solicitud de inicio de eliminación de formulario", "Inicio de eliminación", TipoAuditoria.Negocio);
                    await _disclaimersRepository.DeleteAsync(disclaimer);
                    await _logger.LogAsync(disclaimer.IdProceso, "Eliminar Disclaimers", disclaimer.ToDisclaimerDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(disclaimer.IdProceso, "Eliminar Disclaimers", disclaimer.ToDisclaimerDTO().ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                }
            }
            if (disclaimer?.IdProceso != null)
            {
                return RedirectToAction("Edit", "Procesos", new { id = disclaimer.IdProceso, idTabs = "disclaimers" });
            }
            else
            {
                return RedirectToAction("Index", "Procesos");
            }
        }
    }
}
