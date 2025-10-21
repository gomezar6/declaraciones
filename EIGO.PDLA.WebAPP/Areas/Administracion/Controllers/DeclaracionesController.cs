#nullable disable
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Helpers;
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
using System.Text;
using System.Text.Json;
using EIGO.PDLA.WebAPP.Services;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;

[Area("Administracion")]
[Authorize(Policy = "AnyGroup")]
[BreadcrumbAdminActionFilter]
[TypeFilter(typeof(ErrorFilter))]
public class DeclaracionesController : Controller
{
    private readonly IDeclaracionesService _declaracionesService;
    private readonly IEntityRepository<Declaracion> _declaracionRepository;
    private readonly IEntityRepository<Formulario> _formularioRepository;
    private readonly IEntityRepository<Funcionario> _funcionarioRepository;
    private readonly IEntityRepository<Parentesco> _parentescoRepository;
    private readonly IEntityRepository<Pais> _paisRepository;
    private readonly IEntityRepository<Ciudad> _ciudadRepository;
    private readonly IEntityRepository<Familiar> _FamiliarRepository;
    private readonly IEntityRepository<Participacion> _participacionRepository;
    private readonly IEntityRepository<TipoDeclaracion> _tipodeclaracionRepository;
    private readonly IEntityRepository<FuncionarioNacionalidad> _funcionarioNacionalidadRepository;
    private readonly IEntityRepository<Proceso> _procesoRepository;
    private readonly IEntityRepository<ProcesosFuncionario> _procesosFuncionarioRepository;
    private readonly IEntityRepository<EstadoDeclaracion> _estadoDeclaracionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPdlaLogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IEntityRepository<Alerta> _AlertaRepository;

    public DeclaracionesController(IDeclaracionesService declaracionesService, IEntityRepository<Declaracion> declaracionRepository, IEntityRepository<Formulario> formularioRepository, IEntityRepository<Funcionario> funcionarioRepository, IEntityRepository<Parentesco> parentescoRepository, IEntityRepository<Pais> paisRepository, IEntityRepository<Ciudad> ciudadRepository, IEntityRepository<Familiar> familiarRepository, IEntityRepository<Participacion> participacionRepository,
      IEntityRepository<FuncionarioNacionalidad> funcionarioNacionalidadRepository, IEntityRepository<TipoDeclaracion> tipodeclaracionRepository,
      IEntityRepository<Proceso> procesoRepository, IEntityRepository<ProcesosFuncionario> procesosFuncionarioRepository, IEntityRepository<EstadoDeclaracion> estadoDeclaracionRepository,
    IPdlaLogger pdlaLogger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IEntityRepository<Alerta> AlertaRepository
        )
    {
        _declaracionRepository = declaracionRepository;
        _formularioRepository = formularioRepository;
        _funcionarioRepository = funcionarioRepository;
        _paisRepository = paisRepository;
        _parentescoRepository = parentescoRepository;
        _ciudadRepository = ciudadRepository;
        _FamiliarRepository = familiarRepository;
        _participacionRepository = participacionRepository;
        _funcionarioNacionalidadRepository = funcionarioNacionalidadRepository;
        _tipodeclaracionRepository = tipodeclaracionRepository;
        _procesoRepository = procesoRepository;
        _procesosFuncionarioRepository = procesosFuncionarioRepository;
        _estadoDeclaracionRepository = estadoDeclaracionRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = pdlaLogger;
        _configuration = configuration;
        _declaracionesService = declaracionesService;
        _AlertaRepository = AlertaRepository;
    }
    // GET: DeclaracionesController
    public async Task<IActionResult> Index()
    {

        var declaracionVM = new DeclaracionDTO
        {
            proceso = (List<ProcesoDto>)((ProcesoRepository)_procesoRepository).GetAllActiveApartirEnProcesoAsync().Result.ToProcesoDTOCollection()
        };



        //ViewData["IdProceso"] = new SelectList(await ((ProcesoRepository)_procesoRepository).GetAllActiveApartirEnProcesoAsync(), "IdProceso", "NombreProceso"+ "IdProceso");
        return View(declaracionVM);
    }
    // GET: DeclaracionesController/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var declaracion = await _declaracionRepository.GetByIdAsync(id.Value);
        if (declaracion == null)
        {
            return NotFound();
        }
       
        var funcionario = await _funcionarioRepository.GetByIdAsync(declaracion.IdFuncionario);
        // var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByEmailAsync(User.Identity.Name);
        var formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
        List<Participacion> participacion = await ((ParticipacionRepository)_participacionRepository).GetByIdDeclaracionAsync(declaracion.IdDeclaracion);
        List<FuncionarioNacionalidad> funcionarioNacionalidad = await ((FuncionarioNacionalidadRepository)_funcionarioNacionalidadRepository).GetListByIdAsync(declaracion.IdFuncionario, declaracion.IdDeclaracion);
        if (funcionario == null)
        {
            return NotFound();
        }
        var declaracionVM = new DeclaracionDTO
        {
            IdCiudad = declaracion.IdCiudad,
            ConfirmacionResponsabilidad = declaracion.ConfirmacionResponsabilidad,
            FechaDeclaracion = declaracion.FechaDeclaracion,
            Cargo = declaracion.Cargo,   //usado en todos menos inversiones. En inversiones se usa funcionario Cargo
            UnidadOrganizacional = funcionario.UnidadOrganizacional,
            IdFuncionario = funcionario.IdFuncionario,
            IdFormulario = declaracion.IdFormulario,
            IdDeclaracion = declaracion.IdDeclaracion,
            IdEstadoDeclaracion = declaracion.IdEstadoDeclaracion,
            RecibidaEnFisico = declaracion.RecibidaEnFisico,
            Funcionario = funcionario.ToFuncionarioDTO(),
            EstadoDeclaracion = declaracion.IdEstadoDeclaracionNavigation.ToEstadoDeclaracionDTO(),
            Formulario = formulario.ToFormularioDTO(),
            CountDisclaimer = formulario.ToFormularioDTO().ProcesoDisclaimerFormulario.Count(),
            Participacion = participacion.Where(s => s.Cargo == "").ToList(),    // se llena los items que cumplen con la condicion de participaciones  Form de inversioens
            Responsabilidad = participacion.Where(s => s.PctAccionario == 0).ToList(),  // se llena los items que cumplen con la condicion de responsaiblidad Form de inversioens
            Nacionalidades = funcionarioNacionalidad
        };
        declaracionVM.Formulario.TipoDeclaracion = formulario.IdTipoDeclaracionNavigation.ToTipoDeclaracionDTO();
        var ciudad = await _ciudadRepository.GetByIdAsync(declaracion.IdCiudad.GetValueOrDefault());
        if (ciudad != null)
        {
            var Pais = await _paisRepository.GetByIdAsync(ciudad.IdPais);
            ViewData["IdPais"] = new SelectList(await _paisRepository.GetAllAsync(), "IdPais", "NombrePais", Pais.IdPais);
        }
        else
        {
            ViewData["IdPais"] = new SelectList(await _paisRepository.GetAllAsync(), "IdPais", "NombrePais");
        }
        ViewData["IdPaises"] = new SelectList(await _paisRepository.GetAllActiveAsync(), "IdPais", "NombrePais");
        ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", declaracion.IdFormulario);
        var Familiares = await ((FamiliarRepository)_FamiliarRepository).GetByIdFuncionarioAsync(funcionario.IdFuncionario);
        ViewData["IdFamiliares"] = new SelectList(Familiares, "IdFamiliar", "NombreFamiliar");
        ViewData["IdParentesco"] = new SelectList(_parentescoRepository.GetAllActiveAsync().Result, "IdParentesco", "NombreParentesco");
        return View(declaracionVM);
    }

    // GET: DeclaracionesController1/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: DeclaracionesController1/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: DeclaracionesController1/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var declaracion = await _declaracionRepository.GetByIdAsync(id.Value);
        if (declaracion == null)
        {
            return NotFound();
        }
        int CUP = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("CUP"));
        if (CUP == 0 || CUP == null)
        {
            CUP = Convert.ToInt32(await GraphHelper.GetUserCUP(_httpContextAccessor.HttpContext));
            if (CUP == 0 || CUP == null)
            {

                _httpContextAccessor.HttpContext.Session.Clear();
                return Redirect("/MicrosoftIdentity/Account/SignOut");
            }

        }
        var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByCUPAsync(CUP);
        // var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByEmailAsync(User.Identity.Name);
        var formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
        List<Participacion> participacion = await ((ParticipacionRepository)_participacionRepository).GetByIdDeclaracionAsync(declaracion.IdDeclaracion);
        List<FuncionarioNacionalidad> funcionarioNacionalidad = await ((FuncionarioNacionalidadRepository)_funcionarioNacionalidadRepository).GetListByDeclaracionFuncionarioAsync(declaracion.IdFuncionario, declaracion.IdDeclaracion);
        declaracion.IdFormularioNavigation.IdTipoDeclaracionNavigation = formulario.IdTipoDeclaracionNavigation;
        if (funcionario == null)
        {
            return NotFound();
        }
        string cargoFormularios = declaracion.Cargo;
        if (cargoFormularios == "" || cargoFormularios == null)
        {
            cargoFormularios = funcionario.Cargo;
        }
        var declaracionVM = new DeclaracionDTO
        {
            IdCiudad = declaracion.IdCiudad,
            ConfirmacionResponsabilidad = declaracion.ConfirmacionResponsabilidad,
            FechaDeclaracion = declaracion.FechaDeclaracion,
            Cargo = cargoFormularios,   //usado en todos menos inversiones. En inversiones se usa funcionario Cargo
            UnidadOrganizacional = funcionario.UnidadOrganizacional,
            IdFuncionario = funcionario.IdFuncionario,
            IdFormulario = declaracion.IdFormulario,
            IdDeclaracion = declaracion.IdDeclaracion,
            IdEstadoDeclaracion = declaracion.IdEstadoDeclaracion,
            RecibidaEnFisico = declaracion.RecibidaEnFisico,
            Funcionario = funcionario.ToFuncionarioDTO(),
            EstadoDeclaracion = declaracion.IdEstadoDeclaracionNavigation.ToEstadoDeclaracionDTO(),
            Formulario = declaracion.IdFormularioNavigation.ToFormularioDTO(),
            CountDisclaimer = declaracion.IdFormularioNavigation.ProcesoDisclaimerFormulario.Count,
            Participacion = participacion.Where(s => s.Cargo == "").ToList(),    // se llena los items que cumplen con la condicion de participaciones  Form de inversioens
            Responsabilidad = participacion.Where(s => s.PctAccionario == 0).ToList(),  // se llena los items que cumplen con la condicion de responsaiblidad Form de inversioens
            Nacionalidades = funcionarioNacionalidad
        };
        var ciudad = await _ciudadRepository.GetByIdAsync(declaracion.IdCiudad.GetValueOrDefault());
        if (ciudad != null)
        {
            var Pais = await _paisRepository.GetByIdAsync(ciudad.IdPais);
            ViewData["IdPais"] = new SelectList(await _paisRepository.GetAllAsync(), "IdPais", "NombrePais", Pais.IdPais);
        }
        else
        {
            ViewData["IdPais"] = new SelectList(await _paisRepository.GetAllAsync(), "IdPais", "NombrePais");
        }
        declaracionVM.FechaDeclaracion = DateTime.Today;
        ViewData["IdPaises"] = new SelectList(await _paisRepository.GetAllActiveAsync(), "IdPais", "NombrePais");
        ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", declaracion.IdFormulario);
        var Familiares = await ((FamiliarRepository)_FamiliarRepository).GetByIdFuncionarioAsync(funcionario.IdFuncionario);
        ViewData["IdFamiliares"] = new SelectList(Familiares, "IdFamiliar", "NombreFamiliar");
        ViewData["IdParentesco"] = new SelectList(_parentescoRepository.GetAllActiveAsync().Result, "IdParentesco", "NombreParentesco");
        return View(declaracionVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // POST: DeclaracionesController1/Edit/5
    public async Task<IActionResult> Edit(int id, [Bind("IdDeclaracion,IdEstadoDeclaracion,IdFormulario,Cargo, UnidadOrganizacional, NombreDeclaracion,Nacionalidad,IdCiudad,FechaDeclaracion,ConfirmacionResponsabilidad,Referencia,RecibidaEnFisico,Funcionario")]
    DeclaracionDTO declaracion)
    {



        declaracion.Funcionario = _funcionarioRepository.GetByIdAsync(declaracion.Funcionario.IdFuncionario).Result.ToFuncionarioDTO();
        declaracion.IdEstadoDeclaracion = 2; // se asigna estatus de diligenciada a la declaracion
        declaracion.UnidadOrganizacional = declaracion.Funcionario.UnidadOrganizacional;
        declaracion.IdFuncionario = declaracion.Funcionario.IdFuncionario;
        declaracion.FechaDeclaracion = DateTime.Today;
        declaracion.RecibidaEnFisico = false;
        ModelState.Clear();
        TryValidateModel(declaracion);
        if (id != declaracion.IdDeclaracion)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
           // await _logger.LogAsync(declaracion.Formulario.IdProceso, "Declaracion:Edición del funcionario", "Solicitud de inicio de Edición del funcionario", "Inicio de Edición", TipoAuditoria.Negocio);

            try
            {

                await _declaracionRepository.UpdateAsync(declaracion.ToDeclaracion());
                var estadoDeclaracion =await _estadoDeclaracionRepository.GetByIdAsync(declaracion.IdEstadoDeclaracion);
                declaracion.EstadoDeclaracion = estadoDeclaracion.ToEstadoDeclaracionDTO();

                var Formulario = await _formularioRepository.GetByIdAsync(declaracion.Formulario.IdFormulario);
                declaracion.Formulario = Formulario.ToFormularioDTO();
                await _logger.LogAsync(declaracion.Formulario.IdProceso, "Diligenciar Declaracion " + declaracion.Formulario.Titulo , declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);

                for (int i = 0; i <= Request.Form.Count; i++)
                {
                    var IdFamiliar = Request.Form["NombreCompleto[" + i + "]"];
                    var IdParticipacion = Request.Form["IdParticipacion[" + i + "]"];
                    var IdDeclaracion = declaracion.IdDeclaracion;
                    var IdPais = Request.Form["Pais[" + i + "]"];
                    var PorcentajeParticipacion = Request.Form["PorcentajeParticipacion[" + i + "]"];
                    var NombreEmpresa = Request.Form["NombreEmpresa[" + i + "]"];
                    var CargoParticipacion = Request.Form["CargoParticipacion[" + i + "]"];
                    var Parentesco = Request.Form["Parentesco[" + i + "]"];
                    var NombreFamiliar = Request.Form["NombreCompleto[" + i + "]"];
                    if (NombreFamiliar == "-1")
                    {
                        NombreFamiliar = Request.Form["NuevaParticipacionNombre[" + i + "]"];
                    }
                    if (PorcentajeParticipacion == "" || PorcentajeParticipacion.Count == 0)
                    {
                        PorcentajeParticipacion = "0";
                    }
                    if (!string.IsNullOrEmpty(IdPais))
                    {
                        if (IdParticipacion == "" || IdParticipacion.Count == 0)
                        {
                            _ = int.TryParse(IdFamiliar, out int intIdFamiliar);
                            _ = int.TryParse(IdPais, out int intIdPais);
                            _ = int.TryParse(Parentesco, out int intIdParentesco);
                            // TODO: Agregar log
                            Participacion participacionInsertada = await _participacionRepository.AddAsync(new Participacion
                            {
                                IdPais = intIdPais,
                                IdDeclaracion = IdDeclaracion,
                                NombreEmpresa = NombreEmpresa,
                                PctAccionario = decimal.Parse(PorcentajeParticipacion),
                                NombreFamiliar = NombreFamiliar,
                                Cargo = CargoParticipacion,
                                IdParentesco = intIdParentesco
                            });
                        }
                        else
                        {
                            // TODO: Es necesario?
                            _ = int.TryParse(IdFamiliar, out int intIdFamiliar);
                            _ = int.TryParse(IdParticipacion, out int intIdParticipacion);
                            var verificarParticipacion = await ((ParticipacionRepository)_participacionRepository).Exist(intIdParticipacion, IdDeclaracion);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

               // await StartProcess(declaracion.Formulario.IdProceso);

                var alerta = await ((AlertaRepository)_AlertaRepository).GetByAlertaDiligenciadaProcesoAsync(declaracion.Formulario.IdProceso);
                if (alerta != null)
                {

                    await StartProcess(declaracion.Formulario.IdProceso);
                }
                else
                {

                    return RedirectToAction("Index");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _declaracionRepository.Exist(declaracion.IdDeclaracion))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Declaraciones");
        }
        ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", declaracion.IdFormulario);
        return View(declaracion);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDeclaraciones([FromBody] DeclaracionNacionalidadAjaxDTO DeclaracionNacionalidad)
    {
        try
        {

            Console.Write(DeclaracionNacionalidad);

            var declaracionBD = await _declaracionRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdDeclaracion);

            DeclaracionNacionalidad.declaracion.Nombres = declaracionBD.Nombres;

            DeclaracionNacionalidad.declaracion.Apellidos = declaracionBD.Apellidos;
            DeclaracionNacionalidad.declaracion.Vicepresidencia = declaracionBD.Vicepresidencia;

            DeclaracionNacionalidad.declaracion.Siglas = declaracionBD.Siglas;

            DeclaracionNacionalidad.declaracion.LugarTrabajo = declaracionBD.LugarTrabajo;


            DeclaracionNacionalidad.declaracion.Funcionario = _funcionarioRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdFuncionario.GetValueOrDefault()).Result.ToFuncionarioDTO();
            DeclaracionNacionalidad.declaracion.IdEstadoDeclaracion = 2; // se asigna estatus de diligenciada a la declaracion
            DeclaracionNacionalidad.declaracion.UnidadOrganizacional = DeclaracionNacionalidad.declaracion.Funcionario.UnidadOrganizacional;
            DeclaracionNacionalidad.declaracion.FechaDeclaracion = DateTime.Today;
            DeclaracionNacionalidad.declaracion.RecibidaEnFisico = false;
            ModelState.Clear();
            TryValidateModel(DeclaracionNacionalidad);


            if (DeclaracionNacionalidad.declaracion.IdDeclaracion == 0)
            {
                return NotFound();
            }
            try
            {
                var estadoDeclaracion = await _estadoDeclaracionRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdEstadoDeclaracion);
                DeclaracionNacionalidad.declaracion.EstadoDeclaracion = estadoDeclaracion.ToEstadoDeclaracionDTO();


                FormularioDTO Formulario = _formularioRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdFormulario).Result.ToFormularioDTO();
                DeclaracionNacionalidad.declaracion.Formulario = Formulario;

                await _declaracionesService.ActualizarNacionalidadAsync(DeclaracionNacionalidad);
                await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario.IdProceso, "Diligenciar Declaracion " + DeclaracionNacionalidad.declaracion.Formulario.Titulo, DeclaracionNacionalidad.declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);

              //  await StartProcess(Formulario.IdProceso);

                var alerta = await ((AlertaRepository)_AlertaRepository).GetByAlertaDiligenciadaProcesoAsync(Formulario.IdProceso);
                if (alerta != null)
                {

                    await StartProcess(Formulario.IdProceso);
                }
                else
                {
                    return Json(new { result = true });

                }

            }
            catch (DbUpdateConcurrencyException)


            {
                if (!await _declaracionRepository.Exist(DeclaracionNacionalidad.declaracion.IdDeclaracion))
                {
                    return NotFound();

                }
                else
                {
                    throw;
                }
            }
        }
        catch (Exception ex)
        {

            var config = TelemetryConfiguration.CreateDefault();
            var client = new TelemetryClient(config);
            client.TrackException(ex);
            Console.Error.WriteLine(ex.ToString());
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
        return Json(new { result = true });
    }
    // GET: DeclaracionesController1/Delete/5
    public ActionResult Delete()
    {
        return View();
    }
    // POST: DeclaracionesController1/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
    public async Task<IActionResult> DeletParticipacion(int IdParticipacion, int IdDeclaracion, int IdPais, int Idparentesco)
    {
        if (IdParticipacion == 0)
        {
            return Json(false);
        }

        var participacion = await _participacionRepository.GetByIdAsync(IdParticipacion);
        var disclaimer = await _participacionRepository.DeleteAsync(new Participacion
        {
            IdParticipacion = IdParticipacion,
            IdParentesco = Idparentesco,
            IdPais = IdPais,
            IdDeclaracion = IdDeclaracion
        });
        var declaracion = await _declaracionRepository.GetByIdAsync(IdDeclaracion);

        var Formulario =await  _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
        declaracion.IdFormularioNavigation = Formulario;

        await _logger.LogAsync(declaracion.IdFormularioNavigation.IdProceso, "Eliminar Participación Declaración " + declaracion.IdFormularioNavigation.Titulo, participacion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);


        return Json(new { disclaimer.Eliminado });
    }
    [HttpGet]
    public async Task<JsonResult> GetFuncionariosbyProcess(int IdProceso)
    {
        try
        {
            //var Proceso = await _procesoRepository.GetByIdAsync(IdProceso);
            if (IdProceso == 0)
            {
                return Json(false);
            }
            else
            {


                var lista = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveByProcesoAsync(IdProceso);
                if (lista.Count() > 0)
                {
                    return Json(new { result = System.Text.Json.JsonSerializer.Serialize(lista.ToProcesoFormularioDTOCollection()) });
                }
                else
                {
                    return Json(false);
                }
            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
    }
    [HttpGet]
    public async Task<JsonResult> GetDeclaracionesbyProcess(int IdProceso, int IdFuncionario)
    {
        try
        {
            bool Proceso = await _procesoRepository.Exist(IdProceso);
            if (IdProceso == 0 || Proceso == false)
            {
                return Json(false);
            }
            else
            {
                bool funcionario = false;
                var lista = new List<Declaracion> { };
                if (IdFuncionario > 0)
                {
                    funcionario = await _funcionarioRepository.Exist(IdFuncionario);
                    if (!funcionario)
                    {
                        return Json(false);
                    }
                    lista = await ((DeclaracionRepository)_declaracionRepository).GetAllActivebyFuncionarioAsync(IdFuncionario);
                }
                else
                {
                    lista = await ((DeclaracionRepository)_declaracionRepository).GetAllActivebyProcesoAsync(IdProceso);
                }
                if (lista == null)
                {
                    return Json(false);
                }
                else
                {
                    lista = lista.Where(p => p.IdFormularioNavigation.IdProceso == IdProceso).ToList();
                    if (lista.Any())
                    {
                        return Json(new { result = System.Text.Json.JsonSerializer.Serialize(lista.ToDeclaracionCollection()) });
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
    }
    [HttpGet]
    public async Task<JsonResult> GetEstadoDeclaraciones()
    {
        var estadoDeclaracion = await _estadoDeclaracionRepository.GetAllActiveAsync();
        if (estadoDeclaracion != null)
        {
            return Json(new { estadoDeclaracion });
        }
        return Json(false);
    }
    [HttpPost]
    public async Task<IActionResult> ActualizarEstadoDeclaracion(int idDeclaracion, int idEstadoDeclaracion)
    {
        try
        {
            Declaracion declaracion = await _declaracionRepository.GetByIdAsync(idDeclaracion);
            declaracion.IdEstadoDeclaracion = idEstadoDeclaracion;

            declaracion.FechaDeclaracion = null;

            if (declaracion != null)
            {
                 await _declaracionRepository.UpdateAsync(declaracion);



                declaracion.IdEstadoDeclaracionNavigation = await _estadoDeclaracionRepository.GetByIdAsync(idEstadoDeclaracion);


                var funcionario =await _funcionarioRepository.GetByIdAsync(declaracion.IdFuncionario);
                declaracion.IdFuncionarioNavigation = funcionario;
                await _logger.LogAsync(declaracion.IdFormularioNavigation.IdProceso, "Actualizar Estado Declaración " + declaracion.IdFormularioNavigation.Titulo, declaracion.ToDeclaracionDTO().ToAuditoria(), "exitoso", TipoAuditoria.Negocio);
            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
        return Json(new { result = true });
    }
    [HttpPost]
    public async Task<IActionResult> ActualizarRecibidaEnFisicoDeclaracion(int idDeclaracion, bool recibidaEnFisico)
    {
        try
        {
            Declaracion declaracion = await _declaracionRepository.GetByIdAsync(idDeclaracion);
            declaracion.RecibidaEnFisico = recibidaEnFisico;
            if (declaracion != null)
            {
                await _declaracionRepository.UpdateAsync(declaracion);
                

                var funcionario = await _funcionarioRepository.GetByIdAsync(declaracion.IdFuncionario);
                declaracion.IdFuncionarioNavigation = funcionario;
                await _logger.LogAsync(declaracion.IdFormularioNavigation.IdProceso, "Actualizar Declaración Recibida en Fisico " + declaracion.IdFormularioNavigation.Titulo, declaracion.ToDeclaracionDTO().ToAuditoria(), "exitoso", TipoAuditoria.Negocio);
            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
        return Json(new { result = true });
    }

    public async Task<IActionResult> GetComentariosDeclaracion(int idDeclaracion)
    {
        if (idDeclaracion == 0)
        {
            return NotFound();
        }


        try
        {
            Declaracion declaracion = await _declaracionRepository.GetByIdAsync(idDeclaracion);
            return Json(new { result = declaracion.Observaciones });
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }

    }


    [HttpPost]
    public async Task<IActionResult> CreateNewCommentDeclaracion([FromBody] DeclaracionObservacionDTO DeclaracionObservacion)
    {
        try
        {
            Declaracion declaracion = await _declaracionRepository.GetByIdAsync(DeclaracionObservacion.IdDeclaracion);
            declaracion.Observaciones = declaracion.Observaciones + "#L#" + DateTime.Now + "#C#" + DeclaracionObservacion.Observaciones;
            if (declaracion != null)
            {
                await _declaracionRepository.UpdateAsync(declaracion);


            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
        return Json(new { result = true });
    }

    public async Task<IActionResult> DetailsPreview(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var funcionario = new Funcionario();
        var formulario = await _formularioRepository.GetByIdAsync(id.GetValueOrDefault());

        var estadoDeclaracion = await _estadoDeclaracionRepository.GetByIdAsync(1);

        List<Participacion> participacion = new List<Participacion>();
        List<FuncionarioNacionalidad> funcionarioNacionalidad = new List<FuncionarioNacionalidad>();
        var declaracionVM = new DeclaracionDTO
        {
            IdFuncionario = funcionario.IdFuncionario,
            IdFormulario = formulario.IdFormulario,

            IdEstadoDeclaracion = 1,

            Funcionario = funcionario.ToFuncionarioDTO(),
            EstadoDeclaracion = estadoDeclaracion.ToEstadoDeclaracionDTO(),
            Formulario = formulario.ToFormularioDTO(),

            CountDisclaimer = formulario.ProcesoDisclaimerFormulario.Count,
            Participacion = participacion.Where(s => s.Cargo == "").ToList(),    // se llena los items que cumplen con la condicion de participaciones  Form de inversioens
            Responsabilidad = participacion.Where(s => s.PctAccionario == 0).ToList(),  // se llena los items que cumplen con la condicion de responsaiblidad Form de inversioens
            Nacionalidades = funcionarioNacionalidad
        };
        declaracionVM.Formulario.Preview = true;
        declaracionVM.Formulario.TipoDeclaracion = formulario.IdTipoDeclaracionNavigation.ToTipoDeclaracionDTO();

        ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", formulario.IdFormulario);
        var Familiares = await ((FamiliarRepository)_FamiliarRepository).GetByIdFuncionarioAsync(funcionario.IdFuncionario);
        ViewData["IdFamiliares"] = new SelectList(Familiares, "IdFamiliar", "NombreFamiliar");
        ViewData["IdParentesco"] = new SelectList(_parentescoRepository.GetAllActiveAsync().Result, "IdParentesco", "NombreParentesco");
        return View(declaracionVM);
    }

    public async Task<IActionResult> PreviewDeclaracion(int idDeclaracion, bool recibidaEnFisico)
    {
        try
        {
            Declaracion declaracion = await _declaracionRepository.GetByIdAsync(idDeclaracion);
            declaracion.RecibidaEnFisico = recibidaEnFisico;
            if (declaracion != null)
            {
                await _declaracionRepository.UpdateAsync(declaracion);
            }
        }
        catch (Exception ex)
        {
            // TODO: Log error
            Console.Write(ex.ToString());
            return Json(new { result = false });
        }
        return Json(new { result = true });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartProcess(int IdProceso)
    {
        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
        {
            HttpClient httpClient = new();
            try
            {

                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
               
                Console.Error.WriteLine("Iniciando carga del correo");


                var response = await httpClient.PostAsync(
                    _configuration["Application:AlertServiceURL"],
                    new StringContent(JsonSerializer.Serialize("{ \"IdProceso\": " + IdProceso + ", \"EmailUsuario\": \"" + User.Identity.Name + "\" }"), Encoding.UTF8, "application/json")
                    );

                if (response.IsSuccessStatusCode)
                {
                   await _logger.LogAsync(IdProceso, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", "Exitoso", TipoAuditoria.Sistema);
                    return Json(new { succeed = true, status = 0 });
                }
                else
                {
                    await _logger.LogAsync(IdProceso, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", $"Fallido: Status {response.StatusCode} {await response.Content.ReadAsStringAsync()}", TipoAuditoria.Sistema);
       
                    return Json(new { succeed = false, status = 2 });
                }
            }
            catch (Exception ex)
            {
                await _logger.LogAsync(IdProceso, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", $"Fallido: Status {ex.Message}", TipoAuditoria.Sistema);
              //  await _logger.LogAsync(null, "Inicio de llamado de alerta", "Solicitud de  alerta diligenciada", $"Fallido: ERROR {ex.Message}", TipoAuditoria.Sistema);
                // TODO: Change to DI
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);
                return Json(new { succeed = false, status = 1 });
            }
        }
        await _logger.LogAsync(IdProceso, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", $"Fallido: sin respuesta válida", TipoAuditoria.Sistema);
      //  await _logger.LogAsync(null, "Inicio de llamado de alerta", "Solicitud de  alerta diligenciada", "Fallido: sin respuesta válida", TipoAuditoria.Sistema);
        return Json(new { succeed = false, status = 2 });
    }

}
