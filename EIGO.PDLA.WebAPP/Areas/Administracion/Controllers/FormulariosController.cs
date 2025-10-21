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
using static System.Net.Mime.MediaTypeNames;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Policy = "GroupAdmin")]
    [BreadcrumbAdminActionFilter]
    [TypeFilter(typeof(ErrorFilter))]
    public class FormulariosController : Controller
    {
        private readonly IEntityRepository<Proceso> _procesosRepository;
        private readonly IEntityRepository<Formulario> _formularioRepository;
        private readonly IEntityRepository<TipoDeclaracion> _tipoDeclaracionRepository;
        private readonly IEntityRepository<Disclaimer> _disclaimerRepository;
        private readonly IEntityRepository<ProcesoDisclaimerFormulario> _procesoDisclaimerFormularioRepository;
        private readonly IPdlaLogger _logger;
        public FormulariosController(IEntityRepository<Formulario> formularioRepository, IEntityRepository<Proceso> procesosRepository
            , IEntityRepository<TipoDeclaracion> tipoDeclaracionRepository, IEntityRepository<ProcesoDisclaimerFormulario> procesoDisclaimerFormularioRepository, IEntityRepository<Disclaimer> disclaimerRepository,

        IPdlaLogger pdlaLogger
            )
        {
            _formularioRepository = formularioRepository;
            _procesosRepository = procesosRepository;
            _tipoDeclaracionRepository = tipoDeclaracionRepository;
            _procesoDisclaimerFormularioRepository = procesoDisclaimerFormularioRepository;
            _disclaimerRepository = disclaimerRepository;

            _logger = pdlaLogger;
        }
        // GET: FormulariosController1
        public async Task<IActionResult> Index()
        {
            return View(await _formularioRepository.GetAllActiveAsync());
        }
        // GET: FormulariosController1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var formulario = await _formularioRepository.GetByIdAsync(id.Value);
            if (formulario == null)
            {
                return NotFound();
            }
            ViewData["ControllerProcesoBack"] = "Details";

            if (formulario.IdProcesoNavigation.IdEstadoProceso == 1)
            {
                ViewData["ControllerProcesoBack"] = "Edit";
            }

            ViewData["SelectedValue"] = formulario.IdProceso;
            var formularioVM = new FormularioDTO
            {
                ProcesoDisclaimerFormulario = formulario.ProcesoDisclaimerFormulario.ToProcesoDisclaimerFormularioDTOCollection(),
                Encabezado = formulario.Encabezado,
                IdProceso = formulario.IdProceso,
                PiePagina = formulario.PiePagina,
                RecibirEnFisico = formulario.RecibirEnFisico,
                Texto1 = formulario.Texto1,
                Texto2 = formulario.Texto2,
                Texto3 = formulario.Texto3,
                Texto4 = formulario.Texto4,
                Texto5 = formulario.Texto5,
                Titulo = formulario.Titulo,
                VersionFormulario = formulario.VersionFormulario,
                IdFormulario = formulario.IdFormulario,
                IdTipoDeclaracion = formulario.IdTipoDeclaracion
                //Disclaimer = _disclaimerRepository.GetAllAsync().Result.ToDisclaimerDTOCollection()
            };
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", formulario.IdProceso);
            ViewData["IdTipo"] = new SelectList(_tipoDeclaracionRepository.GetAllAsync().Result, "IdTipo", "NombreDeclaracion", formulario.IdTipoDeclaracion);
            ViewData["IdDisclaimer"] = new SelectList(_disclaimerRepository.GetAllAsync().Result.Where(p => p.IdProceso == formulario.IdProceso && p.Eliminado == false), "IdDisclaimer", "Titulo");
            ViewBag.Editable = formulario.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(formularioVM);
        }
        // GET: FormulariosController1/Create
        public IActionResult Create(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ViewData["IdProceso"] = new SelectList(_procesosRepository.GetAllAsync().Result, "IdProceso", "NombreProceso", id);
            ViewData["IdTipo"] = new SelectList(_tipoDeclaracionRepository.GetAllAsync().Result, "IdTipo", "NombreDeclaracion");
            ViewData["IdDisclaimer"] = new SelectList(_disclaimerRepository.GetAllAsync().Result.Where(p => p.IdProceso == id && p.Eliminado == false), "IdDisclaimer", "Titulo");
            ViewData["SelectedValue"] = id;
            return View();
        }
        // POST: administracion/Disclaimers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFormulario,Preview,IdProceso,IdTipoDeclaracion,VersionFormulario,Titulo,TipoDeclaracion,Encabezado,PiePagina,RecibirEnFisico,Texto1,Texto2,Texto3,Texto4,Texto5")] FormularioDTO formulario)
        {
            Formulario FormularioInserted = new Formulario();
            formulario.ProcesoDisclaimerFormulario = new HashSet<ProcesoDisclaimerFormularioDTO>();
            //formulario.TipoDeclaracion = _tipoDeclaracionRepository.GetByIdAsync( formulario.IdTipoDeclaracion).Result.ToTipoDeclaracionDTO();
            ModelState.Clear();
            TryValidateModel(formulario);
            if (ModelState.IsValid)
            {

                try
                {
                  //  await _logger.LogAsync(formulario.IdProceso, "formulario:Crear", "Solicitud de inicio de Creación de formulario", "Inicio de creación", TipoAuditoria.Negocio);
                    FormularioInserted = await _formularioRepository.AddAsync(formulario.ToFormulario());

                    await _logger.LogAsync(formulario.IdProceso, "Crear Formulario", FormularioInserted.ToFormularioDTO().ToAuditoria(), "exitoso", TipoAuditoria.Negocio);
                    for (int i = 0; i <= Request.Form.Count; i++)
                    {
                        var IdDisclaimer = Request.Form["IdDisclaimer[" + i + "]"];
                        if (!String.IsNullOrEmpty(IdDisclaimer))
                        {
                        //    await _logger.LogAsync(formulario.IdProceso, "formulario:Asociación de formulario y disclaimer", "Formulario: " + FormularioInserted.IdFormulario + " Disclaimer: " + IdDisclaimer, "Inicio de asociación", TipoAuditoria.Negocio);
                            int intIdDisclaimer;
                            Int32.TryParse(IdDisclaimer, out intIdDisclaimer);
                            await _procesoDisclaimerFormularioRepository.AddAsync(new ProcesoDisclaimerFormulario
                            {
                                IdFormulario = FormularioInserted.IdFormulario,
                                IdProceso = FormularioInserted.IdProceso,
                                IdDisclaimer = intIdDisclaimer

                            });

                            var disclaimerInsertado =await _disclaimerRepository.GetByIdAsync(intIdDisclaimer);
                            await _logger.LogAsync(formulario.IdProceso, "Asociar formulario y disclaimer", "Formulario: " + FormularioInserted.ToAuditoria()  + " Disclaimer: " + disclaimerInsertado.ToDisclaimerDTO().ToAuditoria(), "exitoso", TipoAuditoria.Negocio);
                        }
                      
                    }
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(formulario.IdProceso, "Crear Formulario", formulario.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                }

                return RedirectToAction("Edit", "Formularios", new { id = FormularioInserted.IdFormulario, idTabs = "formularios", Save = "true" });



            }
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", formulario.IdProceso);
            ViewData["SelectedValue"] = formulario.IdProceso;
            return View(formulario);
        }
        // GET: FormulariosController1/Edit/5
        public async Task<IActionResult> Edit(int? id, string idTabs, bool save)
        {
            if (save)
            {
                ViewBag.TypeAlert = " alert-success";
                ViewBag.show = " ";
            }
            else
            {
                ViewBag.TypeAlert = " alert-success";
                ViewBag.show = "hiddenElement ";

            }

            if (id == null)
            {
                return NotFound();
            }
            var formulario = await _formularioRepository.GetByIdAsync(id.Value);
            if (formulario == null)
            {
                return NotFound();
            }
            ViewData["ControllerProcesoBack"] = "Details";

            if (formulario.IdProcesoNavigation.IdEstadoProceso == 1)
            {
                ViewData["ControllerProcesoBack"] = "Edit";
            }
            var formularioVM = new FormularioDTO
            {
                ProcesoDisclaimerFormulario = formulario.ProcesoDisclaimerFormulario.ToProcesoDisclaimerFormularioDTOCollection(),
                Encabezado = formulario.Encabezado,
                IdProceso = formulario.IdProceso,
                PiePagina = formulario.PiePagina,
                RecibirEnFisico = formulario.RecibirEnFisico,
                Texto1 = formulario.Texto1,
                Texto2 = formulario.Texto2,
                Texto3 = formulario.Texto3,
                Texto4 = formulario.Texto4,
                Texto5 = formulario.Texto5,
                Titulo = formulario.Titulo,
                VersionFormulario = formulario.VersionFormulario,
                IdFormulario = formulario.IdFormulario,
                IdTipoDeclaracion = formulario.IdTipoDeclaracion
                //Disclaimer = _disclaimerRepository.GetAllAsync().Result.ToDisclaimerDTOCollection()
            };
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", formulario.IdProceso);
            ViewData["IdTipo"] = new SelectList(_tipoDeclaracionRepository.GetAllAsync().Result, "IdTipo", "NombreDeclaracion", formulario.IdTipoDeclaracion);
            ViewData["IdDisclaimer"] = new SelectList(_disclaimerRepository.GetAllAsync().Result.Where(p => p.IdProceso == formulario.IdProceso && p.Eliminado == false), "IdDisclaimer", "Titulo");
            ViewBag.Editable = formulario.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(formularioVM);
        }
        // POST: FormulariosController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFormulario,IdTipoDeclaracion,IdProceso,VersionFormulario,Titulo,TituloDeclaracion,TipoDeclaracion,Encabezado,PiePagina,RecibirEnFisico,Texto1,Texto2,Texto3,Texto4,Texto5")] FormularioDTO formulario)
        {
            formulario.ProcesoDisclaimerFormulario = new HashSet<ProcesoDisclaimerFormularioDTO>();
            ModelState.Clear();
            TryValidateModel(formulario);
            if (id != formulario.IdFormulario)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                  //  await _logger.LogAsync(formulario.IdProceso, "formulario:Actualización", "Solicitud de inicio de actualizacion del formulario: " + formulario.IdFormulario, "Inicio de creación", TipoAuditoria.Negocio);
                    await _formularioRepository.UpdateAsync(formulario.ToFormulario());
                    await _logger.LogAsync(formulario.IdProceso, "Actualización Formulario", formulario.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                    for (int i = 0; i <= Request.Form.Count; i++)
                    {
                        var IdDisclaimer = Request.Form["IdDisclaimer[" + i + "]"];
                        if (!string.IsNullOrEmpty(IdDisclaimer))
                        {
                            int intIdDisclaimer = 0;
                            Int32.TryParse(IdDisclaimer, out intIdDisclaimer);
                         //   await _logger.LogAsync(formulario.IdProceso, "formulario:Asociación de formulario y disclaimer", "Formulario: " + formulario.IdFormulario + " Disclaimer: " + IdDisclaimer, "Inicio de asociación", TipoAuditoria.Negocio);
                            await _procesoDisclaimerFormularioRepository.AddAsync(new ProcesoDisclaimerFormulario
                            {
                                IdFormulario = formulario.IdFormulario,
                                IdProceso = formulario.IdProceso,
                                IdDisclaimer = intIdDisclaimer
                            });
                            var disclaimerInsertado = await _disclaimerRepository.GetByIdAsync(intIdDisclaimer);
                            await _logger.LogAsync(formulario.IdProceso, "Asociar de formulario y disclaimer", "Formulario: " + formulario.IdFormulario + " Disclaimer: " + disclaimerInsertado.ToDisclaimerDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                        }
                     
                    }
                }
                    catch (Exception ex)
                {
                    await _logger.LogAsync(formulario.IdProceso, "Actualización Formulario", formulario.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                    if (!await _formularioRepository.Exist(formulario.IdFormulario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction("Edit", "Procesos", new { id = formulario.IdProceso });
            }
            ViewData["IdTipo"] = new SelectList(_tipoDeclaracionRepository.GetAllAsync().Result, "IdTipo", "NombreDeclaracion", formulario.IdTipoDeclaracion);
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", formulario.IdProceso);
            return RedirectToAction("Edit", "Formularios", new { id = formulario.IdFormulario, idTabs = "formularios", Save = "true" });
        }
        // GET: FormulariosController1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var formulario = await _formularioRepository.GetByIdAsync(id.Value);
            if (formulario == null)
            {
                return NotFound();
            }
            ViewData["ControllerProcesoBack"] = "Details";

            if (formulario.IdProcesoNavigation.IdEstadoProceso == 1)
            {
                ViewData["ControllerProcesoBack"] = "Edit";
            }

            ViewData["SelectedValue"] = formulario.IdProceso;
            var formularioVM = new FormularioDTO
            {
                ProcesoDisclaimerFormulario = formulario.ProcesoDisclaimerFormulario.ToProcesoDisclaimerFormularioDTOCollection(),
                Encabezado = formulario.Encabezado,
                IdProceso = formulario.IdProceso,
                PiePagina = formulario.PiePagina,
                RecibirEnFisico = formulario.RecibirEnFisico,
                Texto1 = formulario.Texto1,
                Texto2 = formulario.Texto2,
                Texto3 = formulario.Texto3,
                Texto4 = formulario.Texto4,
                Texto5 = formulario.Texto5,
                Titulo = formulario.Titulo,
                VersionFormulario = formulario.VersionFormulario,
                IdFormulario = formulario.IdFormulario,
                IdTipoDeclaracion = formulario.IdTipoDeclaracion
                //Disclaimer = _disclaimerRepository.GetAllAsync().Result.ToDisclaimerDTOCollection()
            };
            ViewData["IdProceso"] = new SelectList(await _procesosRepository.GetAllActiveAsync(), "IdProceso", "NombreProceso", formulario.IdProceso);
            ViewData["IdTipo"] = new SelectList(_tipoDeclaracionRepository.GetAllAsync().Result, "IdTipo", "NombreDeclaracion", formulario.IdTipoDeclaracion);
            ViewData["IdDisclaimer"] = new SelectList(_disclaimerRepository.GetAllAsync().Result.Where(p => p.IdProceso == formulario.IdProceso && p.Eliminado == false), "IdDisclaimer", "Titulo");
            ViewBag.Editable = formulario.IdProcesoNavigation.IdEstadoProceso == 1;
            return View(formularioVM);
        }
        // POST: FormulariosController1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formulario = await _formularioRepository.GetByIdAsync(id);
            if (formulario != null)
            {
                try
                {
                    formulario.ProcesoDisclaimerFormulario = null;

                    formulario.IdTipoDeclaracionNavigation = null;
                    formulario.IdProcesoNavigation = null;
                  // await _logger.LogAsync(formulario.IdProceso, "formulario:Eliminar", "Solicitud de inicio de Eliminación de formulario", "Inicio de Eliminación", TipoAuditoria.Negocio);
                    await _formularioRepository.DeleteAsync(formulario);
                   await _logger.LogAsync(formulario.IdProceso, "Eliminar Formulario", formulario.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(formulario.IdProceso, "Eliminar Formulario", "Formulario: " + formulario.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                }
            }
            if (formulario?.IdProceso != null)
            {
                return RedirectToAction("Edit", "Procesos", new { id = formulario.IdProceso, idTabs = "formularios" });
            }
            return RedirectToAction("Index", "Procesos");
        }
        // POST: obtener secciones dado el tipo de declaracion
        [HttpGet]
        public async Task<JsonResult> SeccionesByTipoDeclaracion(int Id)
        {
            var tipoDeclaracion = await _tipoDeclaracionRepository.GetByIdAsync(Id);
            var numSecciones = 0;
            if (tipoDeclaracion != null)
            {
                return Json(new { tipoDeclaracion.NumeroSecciones });
            }
            else
            {
                return Json(new { numSecciones });
            }
        }
        // POST: verificar si existe formularios en un proceso basados en un tipo de declaracion
        [HttpGet]
        public async Task<JsonResult> GetTipoDeclaracionEnFormulario(int IdProceso, int IdTipoDeclaracion)
        {
            if (IdProceso == 0 || IdTipoDeclaracion == 0)
            {
                return Json(false);
            }
            else
            {
                var formulariosbyTipoDeclaracion = await _formularioRepository.GetAllActiveAsync();
                var lista = formulariosbyTipoDeclaracion.ToList().Where(p => p.IdTipoDeclaracion == IdTipoDeclaracion && p.IdProceso == IdProceso);
                var existe = false;
                if (lista.Count() > 0)
                {
                    existe = true;
                }
                return Json(new { existe });
            }
        }
        [HttpGet]
        public async Task<JsonResult> DeleteDisclaimerEnFormulario(int IdProceso, int IdFormulario, int IdDisclaimer)
        {
            if (IdProceso == 0 || IdFormulario == 0 || IdDisclaimer == 0)
            {
                return Json(false);
            }
            else
            {
                try
                {
               //     await _logger.LogAsync(IdProceso, "formulario:Eliminar Formulario Disclaimers", "Formulario: " + IdFormulario + " Disclaimer: " + IdDisclaimer, "Inicio de Eliminación", TipoAuditoria.Negocio);
                    ProcesoDisclaimerFormulario test = new ProcesoDisclaimerFormulario();

                    test.IdFormulario = IdFormulario;
                    test.IdDisclaimer = IdDisclaimer;
                    test.IdProceso = IdProceso;


                    var disclaimerInsertado = await _disclaimerRepository.GetByIdAsync(IdDisclaimer);


                    var ProcesoAEliminar =await  ((ProcesoDisclaimerFormularioRepository)_procesoDisclaimerFormularioRepository).GetByAllPKAsync(test);

                    var formulariosbyTipoDeclaracion = await _procesoDisclaimerFormularioRepository.DeleteAsync(ProcesoAEliminar);

                    await _logger.LogAsync(IdProceso, "Eliminar Formulario Disclaimers", "Formulario: " + IdFormulario + " Disclaimer: " + disclaimerInsertado.ToDisclaimerDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                    return Json(new { formulariosbyTipoDeclaracion.Eliminado });
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(IdProceso, "Eliminar Formulario Disclaimers", "Formulario: " + IdFormulario + " Disclaimer: " + IdDisclaimer, $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                    return Json(false);
                }
            }
        }
    }
}
