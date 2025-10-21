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
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;
[Area("Administracion")]
[Authorize(Policy = "GroupAdmin")]
[BreadcrumbAdminActionFilter]
[TypeFilter(typeof(ErrorFilter))]
public class AlertasController : Controller
{
    private readonly IEntityRepository<Alerta> _alertas;
    private readonly IEntityRepository<Proceso> _procesos;
    private readonly IPdlaLogger _logger;
    public AlertasController(IEntityRepository<Alerta> alertas, IEntityRepository<Proceso> procesos, IPdlaLogger pdlaLogger)
    {
        _alertas = alertas;
        _procesos = procesos;
        _logger = pdlaLogger;
    }

    // GET: administracion/Alertas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alerta = await _alertas.GetByIdAsync(id.Value);
        if (alerta == null)
        {
            return NotFound();
        }
        ViewData["IdProceso"] = new SelectList(await _procesos.GetAllActiveAsync(), "IdProceso", "NombreProceso", alerta.IdProceso);
        ViewData["SelectedValue"] = alerta.IdProceso;
        ViewBag.Editable = alerta.IdProcesoNavigation.IdEstadoProceso == 1;
        return View(alerta.ToAlertaDTO());
    }

    public async Task<IActionResult> Create(int id)
    {
        var procesos = await _procesos.GetAllActiveAsync();
        ViewBag.IdProceso = new SelectList(procesos, "IdProceso", "NombreProceso", id);
        ViewData["SelectedValue"] = id;
        var proceso = procesos.FirstOrDefault(p => p.IdProceso == id);
        AlertaDto alerta = new()
        {
            Fecha = DateTime.UtcNow,
            FechaFinProceso = proceso.FechaFin,
            FechaInicioProceso = proceso.FechaInicio
        };
        return View(alerta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdProceso,Diligenciado,FechaInicioProceso,FechaFinProceso,Estatus,Fecha,Asunto,Titulo,SubTitulo,Cuerpo,AvisoConfidencialidad")] AlertaDto alerta)
    {
        await ValidateDates(alerta);
        if (ModelState.IsValid)
        {
            try
            {
             //   await _logger.LogAsync(alerta.IdProceso, "Alerta:Crear", "Solicitud de inicio de creación de alerta", "Inicio de creación", TipoAuditoria.Negocio);
                Alerta alertaInsertada = await _alertas.AddAsync(alerta.ToAlerta());
                alerta.IdAlerta = alertaInsertada.IdAlerta;
                await _logger.LogAsync(alerta.IdProceso, "Crear Alerta", alerta.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                return RedirectToAction("Edit", "Procesos", new { id = alerta.IdProceso, idTabs = "alertas" });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(alerta.IdProceso, "Crear Alerta", alerta.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);
                Console.Error.WriteLine(ex.ToString());
            }
        }
        ViewBag.IdProceso = new SelectList(await _procesos.GetAllActiveAsync(), "IdProceso", "NombreProceso", alerta.IdProceso);
        ViewData["SelectedValue"] = alerta.IdProceso;
        return View(alerta);
    }

    // GET: administracion/Alertas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alerta = await _alertas.GetByIdAsync(id.Value);
        if (alerta == null)
        {
            return NotFound();
        }
        ViewBag.IdProceso = new SelectList(await _procesos.GetAllActiveAsync(), "IdProceso", "NombreProceso", alerta.IdProceso);
        var procesos = await _procesos.GetAllActiveAsync();
        var proceso = procesos.FirstOrDefault(p => p.IdProceso == alerta.IdProceso);
        var alertaDTO = alerta.ToAlertaDTO();
        ViewBag.Editable = alerta.IdProcesoNavigation.IdEstadoProceso == 1;
        alertaDTO.FechaFinProceso = proceso.FechaFin;
        alertaDTO.FechaInicioProceso = proceso.FechaInicio;
        return View(alertaDTO);
    }

    // POST: administracion/Alertas/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("IdAlerta,IdProceso,Diligenciado,FechaInicioProceso,FechaFinProceso,Estatus,Fecha,Asunto,Titulo,SubTitulo,Cuerpo,AvisoConfidencialidad")] AlertaDto alerta)
    {
        if (id != alerta.IdAlerta)
        {
            return NotFound();
        }
        if (alerta == null)
        {
            return NotFound();
        }
        await ValidateDates(alerta);
        if (ModelState.IsValid)
        {
            try
            {
                //await _logger.LogAsync(alerta.IdProceso, "Alerta:Actualizar", "Solicitud de inicio de actualización de alerta", "Inicio de actualización", TipoAuditoria.Negocio);
                await _alertas.UpdateAsync(alerta.ToAlerta());
                await _logger.LogAsync(alerta.IdProceso, "Actualizar Alerta", alerta.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _logger.LogAsync(alerta.IdProceso, "Actualizar Alerta ", alerta.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);

                if (!await _alertas.Exist(alerta.IdAlerta))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Edit", "Procesos", new { Id = alerta.IdProceso, idTabs = "alertas" });
        }
        ViewBag.IdProceso = new SelectList(await _procesos.GetAllActiveAsync(), "IdProceso", "NombreProceso", alerta.IdProceso);
        var alertaDb = await _alertas.GetByIdAsync(alerta.IdAlerta);
        ViewBag.Editable = alertaDb.IdProcesoNavigation.IdEstadoProceso == 1;
        return View(alerta);
    }

    // GET: administracion/Alertas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var alerta = await _alertas.GetByIdAsync(id.Value);
        if (alerta == null)
        {
            return NotFound();
        }
        ViewBag.Editable = alerta.IdProcesoNavigation.IdEstadoProceso == 1;
        return View(alerta.ToAlertaDTO());
    }

    // POST: administracion/Alertas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var alerta = await _alertas.GetByIdAsync(id);
        try
        {
          //  await _logger.LogAsync(alerta.IdProceso, "Alerta:Eliminar", "Solicitud de inicio de eliminación de alerta", "Inicio de eliminación", TipoAuditoria.Negocio);
            await _alertas.DeleteAsync(alerta);
            await _logger.LogAsync(alerta.IdProceso, "Eliminar Alerta", alerta.ToAlertaDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await _logger.LogAsync(alerta.IdProceso, "Eliminar Alerta", alerta.ToAlertaDTO().ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
            var config = TelemetryConfiguration.CreateDefault();
            var client = new TelemetryClient(config);
            client.TrackException(ex);
        }
        return RedirectToAction("Edit", "Procesos", new { Id = alerta.IdProceso, idTabs = "alertas" });
    }

    private async Task ValidateDates(AlertaDto alerta)
    {
        if (alerta.Diligenciado)
        {
            alerta.Fecha = DateTime.UtcNow;
            return;
        }
        var proceso = await _procesos.GetByIdAsync(alerta.IdProceso);
        if (alerta.Fecha < proceso.FechaInicio || alerta.Fecha > proceso.FechaFin)
        {
            ModelState.AddModelError("Invalid Date", "La fecha es inválida");
        }
    }
}
