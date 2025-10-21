using EIGO.PDLA.Common.Helpers;
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using EIGO.PDLA.WebAPP.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using EIGO.PDLA.WebAPP.Filters;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers
{
    [TypeFilter(typeof(ErrorFilter))]
    public class DeclaracionesFuncionarioController : Controller
    {
        private readonly IDeclaracionesService _declaracionesService;
        private readonly IEntityRepository<Declaracion> _declaracionRepository;
        private readonly IEntityRepository<Alerta> _AlertaRepository;
        private readonly IEntityRepository<Formulario> _formularioRepository;
        private readonly IEntityRepository<Funcionario> _funcionarioRepository;
        private readonly IEntityRepository<Parentesco> _parentescoRepository;
        private readonly IEntityRepository<Pais> _paisRepository;
        private readonly IEntityRepository<CatalogoCargos> _catalogoCargosRepository;
        private readonly IEntityRepository<CatalogoAnios> _catalogoAniosRepository;
        private readonly IEntityRepository<Ciudad> _ciudadRepository;
        private readonly IEntityRepository<Familiar> _FamiliarRepository;
        private readonly IEntityRepository<Participacion> _participacionRepository;
        private readonly IEntityRepository<TipoDeclaracion> _tipodeclaracionRepository;
        private readonly IEntityRepository<FuncionarioNacionalidad> _funcionarioNacionalidadRepository;
        private readonly IEntityRepository<Proceso> _procesoRepository;
        private readonly IEntityRepository<ProcesosFuncionario> _procesosFuncionarioRepository;
        private readonly IEntityRepository<EstadoDeclaracion> _estadoDeclaracionRepository;
        private readonly IPdlaLogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DeclaracionesFuncionarioController(
            IEntityRepository<Declaracion> declaracionRepository,
            IEntityRepository<Formulario> formularioRepository,
            IEntityRepository<Funcionario> funcionarioRepository,
            IEntityRepository<Parentesco> parentescoRepository,
            IEntityRepository<Pais> paisRepository,
            IEntityRepository<CatalogoCargos> catalogoCargosRepository,
            IEntityRepository<CatalogoAnios> catalogoAniosRepository,
            IEntityRepository<Ciudad> ciudadRepository,
            IEntityRepository<Familiar> familiarRepository,
            IEntityRepository<Participacion> participacionRepository,
            IEntityRepository<TipoDeclaracion> tipodeclaracionRepository,
            IEntityRepository<FuncionarioNacionalidad> funcionarioNacionalidadRepository,
            IEntityRepository<Proceso> procesoRepository,
            IEntityRepository<ProcesosFuncionario> procesosFuncionarioRepository,
            IEntityRepository<EstadoDeclaracion> estadoDeclaracionRepository,
            IPdlaLogger pdlaLogger, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IDeclaracionesService declaracionesService,
            IEntityRepository<Alerta> AlertaRepository
            )
        {
            _AlertaRepository = AlertaRepository;
            _declaracionRepository = declaracionRepository;
            _formularioRepository = formularioRepository;
            _funcionarioRepository = funcionarioRepository;
            _paisRepository = paisRepository;
            _catalogoCargosRepository = catalogoCargosRepository;
            _catalogoAniosRepository = catalogoAniosRepository;
            _parentescoRepository = parentescoRepository;
            _ciudadRepository = ciudadRepository;
            _FamiliarRepository = familiarRepository;
            _participacionRepository = participacionRepository;
            _funcionarioNacionalidadRepository = funcionarioNacionalidadRepository;
            _tipodeclaracionRepository = tipodeclaracionRepository;
            _procesoRepository = procesoRepository;
            _procesosFuncionarioRepository = procesosFuncionarioRepository;
            _logger = pdlaLogger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _estadoDeclaracionRepository = estadoDeclaracionRepository;
            _declaracionesService = declaracionesService;
        }

        // GET: DeclaracionesFuncionarioController1
        public async Task<IActionResult> Index()
        {
            DeclaracionDTO declaracion = new DeclaracionDTO();
            try
            {
                int CUP = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("CUP"));
                if (CUP == 0 || CUP == null)
                {
                    var cupInfo = await GraphHelper.GetUserCUP(_httpContextAccessor.HttpContext);
                    CUP = Convert.ToInt32(cupInfo);
                    if (CUP == 0 || CUP == null)
                    {

                        _httpContextAccessor.HttpContext.Session.Clear();
                        return Redirect("/MicrosoftIdentity/Account/SignOut");
                    }
                }
                var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByCUPAsync(CUP);
                if (funcionario == null)
                {
                    return View(declaracion);
                }
                else
                {
                    declaracion.procesosFuncionario =await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveByFuncionarioAsync(funcionario.IdFuncionario);
                    return View(declaracion);
                }
            }
            catch (Exception ex)
            {
                return View(declaracion);
            }
            //ViewData["IdProceso"] = new SelectList(await , "IdProceso", "NombreProceso");

        }
        // GET: DeclaracionesFuncionarioController1/Details/5
        public async Task<IActionResult> Preview(int? id)
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
            //   var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByEmailAsync(User.Identity.Name);

            var funcionario = await _funcionarioRepository.GetByIdAsync(declaracion.IdFuncionario);
            var formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
            List<Participacion> participacion = await ((ParticipacionRepository)_participacionRepository).GetByIdDeclaracionAsync(declaracion.IdDeclaracion);
            List<FuncionarioNacionalidad> funcionarioNacionalidad = await ((FuncionarioNacionalidadRepository)_funcionarioNacionalidadRepository).GetListByIdAsync(declaracion.IdFuncionario,declaracion.IdDeclaracion);
            declaracion.IdFormularioNavigation.IdTipoDeclaracionNavigation = formulario.IdTipoDeclaracionNavigation;
            if (funcionario == null || declaracion == null)
            {
                return NotFound();
            }

            if (declaracion.IdFuncionario != funcionario.IdFuncionario)
            {
                return Forbid();
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
                CountDisclaimer = formulario.ToFormularioDTO().ProcesoDisclaimerFormulario.Count,
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
            ViewData["IdPaises"] = new SelectList(await _paisRepository.GetAllActiveAsync(), "IdPais", "NombrePais");
            ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", declaracion.IdFormulario);
            var Familiares = await ((FamiliarRepository)_FamiliarRepository).GetByIdFuncionarioAsync(funcionario.IdFuncionario);
            ViewData["IdFamiliares"] = new SelectList(Familiares, "IdFamiliar", "NombreFamiliar");
            ViewData["IdParentesco"] = new SelectList(_parentescoRepository.GetAllActiveAsync().Result, "IdParentesco", "NombreParentesco");
            return View(declaracionVM);
        }
        // GET: DeclaracionesFuncionarioController1/Details/5
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
            //   var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByEmailAsync(User.Identity.Name);

            var funcionario = await _funcionarioRepository.GetByIdAsync(declaracion.IdFuncionario);
            var formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
            List<Participacion> participacion = new List<Participacion>();
            try
            {
                 participacion = await ((ParticipacionRepository)_participacionRepository).GetByIdDeclaracionAsync(declaracion.IdDeclaracion);
            }
            catch(Exception e)
            {

            }
            
            List<FuncionarioNacionalidad> funcionarioNacionalidad = await ((FuncionarioNacionalidadRepository)_funcionarioNacionalidadRepository).GetListByIdAsync(declaracion.IdFuncionario, declaracion.IdDeclaracion);
            declaracion.IdFormularioNavigation.IdTipoDeclaracionNavigation = formulario.IdTipoDeclaracionNavigation;
            if (funcionario == null || declaracion == null)
            {
                return NotFound();
            }

            if (declaracion.IdFuncionario != funcionario.IdFuncionario)
            {
                return Forbid();
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
                CountDisclaimer = formulario.ToFormularioDTO().ProcesoDisclaimerFormulario.Count,
                Participacion = participacion.Where(s => s.Cargo == "").ToList(),    // se llena los items que cumplen con la condicion de participaciones  Form de inversioens
                Responsabilidad = participacion.Where(s => s.PctAccionario == 0).ToList(),  // se llena los items que cumplen con la condicion de responsaiblidad Form de inversioens
                Nacionalidades = funcionarioNacionalidad,
                sJustificacion = declaracion.sJustificacion,
                bConflictoInteres = declaracion.bConflictoInteres
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
            ViewData["IdPaises"] = new SelectList(await _paisRepository.GetAllActiveAsync(), "IdPais", "NombrePais");
            ViewData["idFormulario"] = new SelectList(await _formularioRepository.GetAllActiveAsync(), "IdFormulario", "TituloDeclaracion", declaracion.IdFormulario);
            var Familiares = await ((FamiliarRepository)_FamiliarRepository).GetByIdFuncionarioAsync(funcionario.IdFuncionario);
            ViewData["IdFamiliares"] = new SelectList(Familiares, "IdFamiliar", "NombreFamiliar");
            ViewData["IdParentesco"] = new SelectList(_parentescoRepository.GetAllActiveAsync().Result, "IdParentesco", "NombreParentesco");
            return View(declaracionVM);
        }
        // GET: DeclaracionesFuncionarioController1/Create
        public ActionResult Create()
        {
            return Forbid();
        }
        // POST: DeclaracionesFuncionarioController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return Forbid();
            }
            catch
            {
                return Forbid();
            }
        }
        // GET: DeclaracionesFuncionarioController1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var declaracion = await _declaracionRepository.GetByIdAsync(id.Value);
            if (declaracion == null || declaracion.IdEstadoDeclaracion != 1)
            {
                return Forbid();
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
            //  var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByEmailAsync(User.Identity.Name);

            if (declaracion.IdFuncionario != funcionario.IdFuncionario)
            {
                return Forbid();
            }
            var formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);

            if (formulario.IdProcesoNavigation.IdEstadoProcesoNavigation.NombreEstadoProceso == "Cerrado" ||
                formulario.IdProcesoNavigation.IdEstadoProcesoNavigation.NombreEstadoProceso == "Cerrado anticipado"
                )
            {
                return Forbid();

            }

            List<Participacion> participacion = new List<Participacion>();
            try
            {
               participacion = await ((ParticipacionRepository)_participacionRepository).GetByIdDeclaracionAsync(declaracion.IdDeclaracion);
            }
            catch (Exception e) { }

           
            List<FuncionarioNacionalidad> funcionarioNacionalidad = await ((FuncionarioNacionalidadRepository)_funcionarioNacionalidadRepository).GetListByDeclaracionFuncionarioAsync(declaracion.IdFuncionario, declaracion.IdDeclaracion);
            declaracion.IdFormularioNavigation.IdTipoDeclaracionNavigation = formulario.IdTipoDeclaracionNavigation;
            if (funcionario == null || declaracion == null)
            {
                return NotFound();
            }
            string cargoFormularios = funcionario.Cargo;//declaracion.Cargo;
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
                Formulario = formulario.ToFormularioDTO(),
                CountDisclaimer = formulario.ToFormularioDTO().ProcesoDisclaimerFormulario.Count(),
                Participacion = participacion.Where(s => s.Cargo == "").ToList(),    // se llena los items que cumplen con la condicion de participaciones  Form de inversioens
                Responsabilidad = participacion.Where(s => s.PctAccionario == 0).ToList(),  // se llena los items que cumplen con la condicion de responsaiblidad Form de inversioens
                Nacionalidades = funcionarioNacionalidad,
                 sJustificacion = declaracion.sJustificacion,
                 bConflictoInteres = declaracion.bConflictoInteres
            };
            var ciudad = await _ciudadRepository.GetByIdAsync(declaracion.IdCiudad.GetValueOrDefault());
            if (ciudad != null)
            {

                var Pais = await _paisRepository.GetByIdAsync(ciudad.IdPais);

                ViewData["IdPais"] = new SelectList(await _paisRepository.GetAllAsync(), "IdPais", "NombrePais", Pais.IdPais);
            }
            else
            {
                Pais pais = new()
                {
                    IdPais = 0,
                    NombrePais = "Seleccione"
                };
                List<Pais> paisList = await _paisRepository.GetAllAsync();
                paisList.Add(pais);
                ViewData["IdPais"] = new SelectList(paisList, "IdPais", "NombrePais", 0);
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
        // POST: DeclaracionesFuncionarioController1/Edit/5
        public async Task<IActionResult> Edit(int id, [Bind("IdDeclaracion,IdEstadoDeclaracion,IdFormulario,Cargo, UnidadOrganizacional, NombreDeclaracion,Nacionalidad,IdCiudad,FechaDeclaracion,ConfirmacionResponsabilidad,Referencia,RecibidaEnFisico,Funcionario,bConflictoInteres,sJustificacion")]
        DeclaracionDTO declaracion)
        {
            var declaracionBD = await _declaracionRepository.GetByIdAsync(id);
            
            declaracion.Funcionario = _funcionarioRepository.GetByIdAsync(declaracion.Funcionario.IdFuncionario).Result.ToFuncionarioDTO();
            declaracion.IdEstadoDeclaracion = 2; // se asigna estatus de diligenciada a la declaracion
            declaracion.UnidadOrganizacional = declaracion.Funcionario.UnidadOrganizacional;
            declaracion.IdFuncionario = declaracion.Funcionario.IdFuncionario;
            declaracion.FechaDeclaracion = DateTime.Today;
            declaracion.RecibidaEnFisico = false;
            declaracion.Nombres = declaracion.Funcionario.Nombres;
            declaracion.Apellidos = declaracion.Funcionario.Apellidos;
            declaracion.Vicepresidencia = declaracionBD.Vicepresidencia;
            declaracion.Siglas = declaracionBD.Siglas;
            declaracion.LugarTrabajo = declaracionBD.LugarTrabajo;
           

            ModelState.Clear();
            TryValidateModel(declaracion);

            if (id != declaracion.IdDeclaracion)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    List<CatalogoAnios> catalogAniosList = await ((CatalogoAniosRepository)_catalogoAniosRepository).GetAllAsync();
                    await _declaracionesService.GenerarAsync(declaracion, Request.Form, catalogAniosList);
                    declaracion.EstadoDeclaracion =  _estadoDeclaracionRepository.GetByIdAsync(declaracion.IdEstadoDeclaracion).Result.ToEstadoDeclaracionDTO();
                    var estadoDeclaracion = await _estadoDeclaracionRepository.GetByIdAsync(declaracion.IdEstadoDeclaracion);
                    declaracion.EstadoDeclaracion = estadoDeclaracion.ToEstadoDeclaracionDTO();

                    var Formulario = await _formularioRepository.GetByIdAsync(declaracion.IdFormulario);
                    declaracion.Formulario = Formulario.ToFormularioDTO();
                    await _logger.LogAsync(declaracion.Formulario.IdProceso, "Diligenciar Declaracion " + declaracion.Formulario.Titulo, declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);




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
                catch (Exception ex)
                {
                    declaracion.EstadoDeclaracion = _estadoDeclaracionRepository.GetByIdAsync(declaracion.IdEstadoDeclaracion).Result.ToEstadoDeclaracionDTO();
                    await _logger.LogAsync(declaracion.Formulario?.IdProceso, "Diligenciar Declaracion", declaracion.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);

                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);

                    if (!await _declaracionRepository.Exist(declaracion.IdDeclaracion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
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
                DeclaracionNacionalidad.declaracion.Funcionario = _funcionarioRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdFuncionario.GetValueOrDefault()).Result.ToFuncionarioDTO();
                DeclaracionNacionalidad.declaracion.IdEstadoDeclaracion = 2; // se asigna estatus de diligenciada a la declaracion
                DeclaracionNacionalidad.declaracion.UnidadOrganizacional = DeclaracionNacionalidad.declaracion.Funcionario.UnidadOrganizacional;
                DeclaracionNacionalidad.declaracion.FechaDeclaracion = DateTime.Today;
                DeclaracionNacionalidad.declaracion.RecibidaEnFisico = false;
                ModelState.Clear();
                TryValidateModel(DeclaracionNacionalidad);
                if (DeclaracionNacionalidad.declaracion.IdDeclaracion == null || DeclaracionNacionalidad.declaracion.IdDeclaracion == 0)
                {
                    return NotFound();
                }
                try
                {
                    await _declaracionesService.ActualizarNacionalidadAsync(DeclaracionNacionalidad);
                    DeclaracionNacionalidad.declaracion.EstadoDeclaracion = _estadoDeclaracionRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdEstadoDeclaracion).Result.ToEstadoDeclaracionDTO();
                    FormularioDTO Formulario = _formularioRepository.GetByIdAsync(DeclaracionNacionalidad.declaracion.IdFormulario).Result.ToFormularioDTO();
                    DeclaracionNacionalidad.declaracion.Formulario = Formulario;


                    await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario.IdProceso, "Diligenciar Declaracion " + DeclaracionNacionalidad.declaracion.Formulario.Titulo, DeclaracionNacionalidad.declaracion.ToAuditoria(), "exitoso", TipoAuditoria.Negocio);

                    var alerta = await ((AlertaRepository)_AlertaRepository).GetByAlertaDiligenciadaProcesoAsync(DeclaracionNacionalidad.declaracion.Formulario.IdProceso);
                    if (alerta != null)
                    {

                        await StartProcess(DeclaracionNacionalidad.declaracion.Formulario.IdProceso);
                    }
                    else
                    {
                        return RedirectToAction("Index");

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
                await _logger.LogAsync(DeclaracionNacionalidad.declaracion.Formulario?.IdProceso, "Diligenciar Declaracion", DeclaracionNacionalidad.declaracion.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
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
                        await _logger.LogAsync(IdProceso, "Enviar Alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", "Exitoso", TipoAuditoria.Sistema);
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
                    // TODO: Change to DI
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                    return Json(new { succeed = false, status = 1 });
                }
            }

            await _logger.LogAsync(IdProceso, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", $"Fallido: sin respuesta válida", TipoAuditoria.Sistema);
            return Json(new { succeed = false, status = 2 });
        }
        // GET: DeclaracionesFuncionarioController1/Delete/5
        public ActionResult Delete()
        {
            return Forbid();
        }
        // POST: DeclaracionesFuncionarioController1/Delete/5
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
            var disclaimer = await _participacionRepository.DeleteAsync(new Participacion
            {
                IdParticipacion = IdParticipacion,
                IdParentesco = Idparentesco,
                IdPais = IdPais,
                IdDeclaracion = IdDeclaracion
            });
            return Json(new { disclaimer.Eliminado });
        }
        [HttpGet]
        public async Task<JsonResult> GetDeclaracionesbyProcess(int IdProceso)
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
                    int CUP = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("CUP"));
                    if (CUP == 0 || CUP == null)
                    {
                        CUP = Convert.ToInt32(await GraphHelper.GetUserCUP(_httpContextAccessor.HttpContext));
                        if (CUP == 0 || CUP == null)
                        {

                            _httpContextAccessor.HttpContext.Session.Clear();
                            return Json("salir");
                        }


                    }
                    var funcionario = await ((FuncionarioRepository)_funcionarioRepository).GetByCUPAsync(CUP);

                    var lista = await ((DeclaracionRepository)_declaracionRepository).GetAllActivebyFuncionarioAsync(funcionario.IdFuncionario);
                    lista = lista.Where(p => p.IdFormularioNavigation.IdProceso == IdProceso).ToList();
                    if (lista.Count > 0)
                    {
                        return Json(new { result = System.Text.Json.JsonSerializer.Serialize(lista.ToDeclaracionCollection()) });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAlert(int IdProceso)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                HttpClient httpClient = new();
                try
                {
                    var response = await httpClient.PostAsync(
                        _configuration["Application:AlertServiceURL"],
                        new StringContent(JsonSerializer.Serialize("{ \"IdProceso\": " + IdProceso + ", \"EmailUsuario\": \"" + User.Identity.Name + "\" }"), Encoding.UTF8, "application/json")
                        );

                    if (response.IsSuccessStatusCode)
                    {
                        await _logger.LogAsync(null, "Enviar alerta Diligenciada", "Solicitud para el envío de la alerta diligenciada", "Exitoso", TipoAuditoria.Sistema);
                        return Json(new { succeed = true, status = 0 });
                    }
                    else
                    {
                        await _logger.LogAsync(
                            null,
                            "Inicio de sincronización",
                            "Solicitud de inicio de sincronización",
                            $"Fallido: Status {response.StatusCode} {await response.Content.ReadAsStringAsync()}", TipoAuditoria.Sistema);
                        return Json(new { succeed = false, status = 2 });
                    }
                }
                catch (Exception ex)
                {
                    await _logger.LogAsync(null, "Enviar alerta Diligenciada", "Solicitud de  alerta diligenciada", $"Fallido: ERROR {ex.Message}", TipoAuditoria.Sistema);
                    // TODO: Change to DI
                    var config = TelemetryConfiguration.CreateDefault();
                    var client = new TelemetryClient(config);
                    client.TrackException(ex);
                    return Json(new { succeed = false, status = 1 });
                }
            }

            await _logger.LogAsync(null, "Enviar alerta Diligenciada", "Solicitud de  alerta diligenciada", "Fallido: sin respuesta válida", TipoAuditoria.Sistema);
            return Json(new { succeed = false, status = 2 });
        }
    }
}
