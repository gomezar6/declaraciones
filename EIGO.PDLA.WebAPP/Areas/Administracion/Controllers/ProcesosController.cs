using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using EIGO.PDLA.WebAPP.Filters;
using EIGO.PDLA.WebAPP.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;
[Area("Administracion")]
[Authorize(Policy = "GroupAdmin")]
[BreadcrumbAdminActionFilter]

[TypeFilter(typeof(ErrorFilter))]
public class ProcesosController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IEntityRepository<Alerta> _alertaRepository;
    private readonly IEntityRepository<Disclaimer> _disclaimerRepository;
    private readonly IEntityRepository<EstadoProceso> _estadoProceso;
    private readonly IEntityRepository<Formulario> _formularioRepository;
    private readonly IEntityRepository<Funcionario> _funcionariosRepository;
    private readonly IEntityRepository<Proceso> _procesosRepository;
    private readonly IEntityRepository<ProcesosFuncionario> _procesosFuncionarioRepository;
    private readonly IEntityRepository<ProcesoDisclaimerFormulario> _procesoDisclaimerFormularioRepository;
    private readonly IPdlaLogger _logger;
    public ProcesosController(
        IConfiguration configuration,
        IEntityRepository<Alerta> alertaRepository,
        IEntityRepository<Disclaimer> disclamerRepository,
        IEntityRepository<EstadoProceso> estadoProceso,
        IEntityRepository<Formulario> formularioRepository,
        IEntityRepository<Funcionario> funcionariosRepository,
        IEntityRepository<Proceso> procesosRepository,
        IEntityRepository<ProcesosFuncionario> procesosFuncionarioRepository,
        IEntityRepository<ProcesoDisclaimerFormulario> procesoDisclaimerFormulario,
        IPdlaLogger pdlaLogger)
    {
        _alertaRepository = alertaRepository;
        _disclaimerRepository = disclamerRepository;
        _estadoProceso = estadoProceso;
        _formularioRepository = formularioRepository;
        _funcionariosRepository = funcionariosRepository;
        _procesosRepository = procesosRepository;
        _procesosFuncionarioRepository = procesosFuncionarioRepository;
        _procesoDisclaimerFormularioRepository = procesoDisclaimerFormulario;
        _configuration = configuration;
        _logger = pdlaLogger;
    }

    // GET: administracion/Procesos
    public async Task<IActionResult> Index()
    {
        var procesos = await _procesosRepository.GetAllActiveAsync();
        return View(procesos.ToProcesoDTOCollection());
    }

    // GET: administracion/Procesos/Details/5
    public async Task<IActionResult> Details(int? id, string idTabs)
    {

        if (idTabs == "alertas")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "show active";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "disclaimers")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "show active";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";

        }
        else if (idTabs == "formularios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "show active";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "auditoria")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "show active";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "funcionarios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "show active";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "Cumplimiento")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "show active";
        }
        else
        {
            ViewBag.proceso = "show active";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }


        if (id == null)
        {
            return NotFound();
        }

        var proceso = await _procesosRepository.GetByIdAsync(id.Value);

        if (proceso == null)
        {
            return NotFound();
        }

        var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == id);

        var procesoVM = new ProcesoDto
        {
            IdProceso = proceso.IdProceso,
            Alertas = proceso.Alerta.ToAlertaDTOCollection(),
            Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection(),
            FechaFin = proceso.FechaFin,
            FechaInicio = proceso.FechaInicio,
            Formularios = proceso.Formularios.ToFormularioDTOCollection(),
            IdEstadoProceso = proceso.IdEstadoProceso,
            EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO(),
            NombreProceso = proceso.NombreProceso,
            Observaciones = proceso.Observaciones,
            Funcionarios = funcionarios.ToFuncionariosCollectionDTO(procesosFuncionarios, proceso.Corporativo.HasValue && proceso.Corporativo.Value),
            Corporativo = proceso.Corporativo.HasValue && proceso.Corporativo.Value
        };

        var (Result, Errors) = await ValidarProcesoAsync(proceso);
        ViewBag.IsValid = Result;
        ViewBag.Errors = Errors;
        ViewBag.rutaCumplimiento = _configuration["Application:CumplimientoProceso"] + "&$filter=Procesos/NombreProceso eq '" + procesoVM.NombreProceso + "'";
        ViewBag.rutaAuditoria = _configuration["Application:AuditoriaProceso"] + "&$filter=Procesos/NombreProceso eq '" + procesoVM.NombreProceso + "'";
        return View(procesoVM);
    }

    // GET: administracion/Procesos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: administracion/Procesos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NombreProceso,Corporativo,FechaInicio,FechaFin,Observaciones")] ProcesoDto proceso)
    {
        await ValidarNombre(proceso);
        if (ModelState.IsValid)
        {

            if (proceso.FechaFin == default(DateTime) || proceso.FechaFin == null)
            {
                proceso.FechaFin = DateTime.MaxValue;

            }
            var porConfigurar = (await _estadoProceso.GetAllAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso.Trim().CompareTo("Por configurar") == 0);
            proceso.IdEstadoProceso = porConfigurar?.IdEstadoProceso ?? 1;
            Proceso procesoInsertado = await _procesosRepository.AddAsync(proceso.ToProceso());
            await _logger.LogAsync(procesoInsertado.IdProceso, "Crear Proceso " + procesoInsertado.NombreProceso, procesoInsertado.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
            return RedirectToAction(nameof(Index));
        }

        return View(proceso);
    }

    public async Task<IActionResult> CreateTemplate(int id)
    {
        var proceso = await _procesosRepository.GetByIdAsync(id);
        if (proceso == null)
        {
            return NotFound();
        }

        if (proceso.FechaFin == default(DateTime) || proceso.FechaFin == null)
        {
            proceso.FechaFin = DateTime.MaxValue;

        }

        var procesoVM = new ProcesoDto
        {
            IdProceso = proceso.IdProceso,
            Alertas = proceso.Alerta.ToAlertaDTOCollection(),
            Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection(),
            FechaFin = proceso.FechaFin,
            FechaInicio = proceso.FechaInicio,
            Formularios = proceso.Formularios.ToFormularioDTOCollection(),
            IdEstadoProceso = proceso.IdEstadoProceso,
            EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO(),
            NombreProceso = proceso.NombreProceso,
            Observaciones = proceso.Observaciones
        };




        return View(procesoVM);
    }

    // POST: administracion/Procesos/CreateTemplate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTemplate(
        [Bind("IdProceso,NombreProceso,FechaInicio,FechaFin,Observaciones")] ProcesoDto proceso)
    {
        var procesoDB = await _procesosRepository.GetByIdAsync(proceso.IdProceso);
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == proceso.IdProceso);

      

        proceso.Alertas = procesoDB.Alerta.ToAlertaDTOCollection();
        proceso.Disclaimers = procesoDB.Disclaimers.ToDisclaimerDTOCollection();
        proceso.Formularios = procesoDB.Formularios.ToFormularioDTOCollection();
        proceso.EstadoProceso = procesoDB.IdEstadoProcesoNavigation.ToEstadoProcesoDTO();




        await ValidarNombre(proceso);

        var (Result, Errors) = await ValidarProcesoTemplateAsync(proceso);

        if (Errors.Count > 0)
        {
            for (int i = 0; i < Errors.Count; i++)
            {

                ModelState.AddModelError("Advertencia", Errors[i]);
            }

        }




        if (ModelState.IsValid)
        {
            var currentProceso = proceso.IdProceso;
            var sourceProcess = await _procesosRepository.GetByIdAsync(proceso.IdProceso);
            var porConfigurar = (await _estadoProceso.GetAllAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso.Trim().CompareTo("Por configurar") == 0);
            proceso.IdProceso = 0;
            proceso.IdEstadoProceso = porConfigurar?.IdEstadoProceso ?? 1;

            if (proceso.FechaFin == default(DateTime) || proceso.FechaFin == null)
            {
                proceso.FechaFin = DateTime.MaxValue;

            }
            var newProcess = await _procesosRepository.AddAsync(proceso.ToProceso());
            await _logger.LogAsync(newProcess.IdProceso, "Crear Proceso Template " + newProcess.NombreProceso, newProcess.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
            if (sourceProcess != null)
            {
                foreach (var alerta in sourceProcess.Alerta)
                {
                    alerta.IdAlerta = 0;
                    alerta.IdProceso = newProcess.IdProceso;
                    alerta.IdProcesoNavigation = null;
                    //   await _logger.LogAsync(newProcess.IdProceso, "Alertas Template :Crear", "Solicitud de inicio de Creación de alertas basado en una plantilla", "Inicio de creación", TipoAuditoria.Negocio);
                    await _alertaRepository.AddAsync(alerta);
                    await _logger.LogAsync(newProcess.IdProceso, "Crear Alertas Proceso Template " + newProcess.NombreProceso, alerta.ToAlertaDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                }
                Dictionary<int, int> mapFormulario = new();
                foreach (var formulario in sourceProcess.Formularios)
                {
                    var currentId = formulario.IdFormulario;
                    formulario.IdFormulario = 0;
                    formulario.IdProceso = newProcess.IdProceso;
                    formulario.IdProcesoNavigation = null;
                    formulario.IdTipoDeclaracionNavigation = null;
                    // await _logger.LogAsync(newProcess.IdProceso, "Formularios Template:Crear", "Solicitud de inicio de Creación de formularios basado en una plantilla", "Inicio de creación", TipoAuditoria.Negocio);
                    var newForm = await _formularioRepository.AddAsync(formulario);
                    await _logger.LogAsync(newProcess.IdProceso, "Crear Formularios Proceso Template " + newProcess.NombreProceso, formulario.ToFormularioDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
                    mapFormulario.Add(currentId, newForm.IdFormulario);
                }

                foreach (var disclaimer in sourceProcess.Disclaimers)
                {
                    var currentId = disclaimer.IdDisclaimer;
                    disclaimer.IdDisclaimer = 0;
                    disclaimer.IdProceso = newProcess.IdProceso;
                    disclaimer.IdProcesoNavigation = null;
                    // await _logger.LogAsync(newProcess.IdProceso, "Disclaimers Template:Crear", "Solicitud de inicio de Creación de disclaimers basado en una plantilla", "Inicio de creación", TipoAuditoria.Negocio);
                    var newDisclaimer = await _disclaimerRepository.AddAsync(disclaimer);
                    await _logger.LogAsync(newProcess.IdProceso, "Crear Disclaimers Proceso Template " + newProcess.NombreProceso, disclaimer.ToDisclaimerDTO().ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);

                }
                var pdfr = await ((ProcesoDisclaimerFormularioRepository)_procesoDisclaimerFormularioRepository).GetbyProcesoAsync(currentProceso);
                // var pelement = pdfr.Where(x => x.IdProceso == currentProceso );

                foreach (var disclaimerProceso in pdfr)
                {
                    var DisclaimerToInsert = await ((DisclaimersRepository)_disclaimerRepository).GetByTextAsync(disclaimerProceso.IdDisclaimerNavigation.Titulo, newProcess.IdProceso);




                    await _procesoDisclaimerFormularioRepository.AddAsync(new ProcesoDisclaimerFormulario
                    {
                        IdDisclaimer = DisclaimerToInsert.IdDisclaimer,
                        IdFormulario = mapFormulario.GetValueOrDefault(disclaimerProceso.IdFormulario),
                        IdProceso = newProcess.IdProceso
                    });
                    await _logger.LogAsync(newProcess.IdProceso, "Crear Asociación Disclaimers Proceso Template " + newProcess.NombreProceso, "Solicitud de inicio de Creación de asociación disclaimers basado en una plantilla", "Exitoso", TipoAuditoria.Negocio);

                }
                //if (pelement != null)
                //    {

                //        await _logger.LogAsync(newProcess.IdProceso, "Asociación Disclaimers Template:Crear", "Solicitud de inicio de Creación de asociación disclaimers basado en una plantilla", "Exitoso", TipoAuditoria.Negocio);
                //    }
                //    else
                //    {
                //        await _logger.LogAsync(newProcess.IdProceso, "Asociación Disclaimers Template:Crear", "Solicitud de inicio de Creación de asociación disclaimers basado en una plantilla", "Fallido", TipoAuditoria.Negocio);
                //    }


            }
            return RedirectToAction(nameof(Index));
        }

        var procesoDb = await _procesosRepository.GetByIdAsync(proceso.IdProceso);
        if (procesoDb != null)
        {
            proceso.Alertas = procesoDb.Alerta.ToAlertaDTOCollection();
            proceso.Disclaimers = procesoDb.Disclaimers.ToDisclaimerDTOCollection();
            proceso.Formularios = procesoDb.Formularios.ToFormularioDTOCollection();
        }


        return View(proceso);
    }

    private async Task ValidarNombre(ProcesoDto proceso)
    {
        var procesos = await ((ProcesoRepository)_procesosRepository).GetAllActiveAsync(p => p.NombreProceso == proceso.NombreProceso);
        if (procesos.Any())
        {
            ModelState.AddModelError("Duplicado", "Existe un proceso con el mismo nombre");
        }
    }

    private async Task ValidarNombreEdit(ProcesoDto proceso)
    {
        var procesos = await ((ProcesoRepository)_procesosRepository).GetAllActiveAsync(p => p.NombreProceso == proceso.NombreProceso && p.IdProceso != proceso.IdProceso);
        if (procesos.Any())
        {
            ModelState.AddModelError("Duplicado", "Existe un proceso con el mismo nombre");
        }
    }

    // GET: administracion/Procesos/Edit/5
    public async Task<IActionResult> Edit(int id, string idTabs)
    {
        var proceso = await _procesosRepository.GetByIdAsync(id);
        if (idTabs == "alertas")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "show active";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "disclaimers")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "show active";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";

        }
        else if (idTabs == "formularios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "show active";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "auditoria")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "show active";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "funcionarios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "show active";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "Cumplimiento")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "show active";
        }
        else
        {
            ViewBag.proceso = "show active";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }

        if (proceso == null)
        {
            return NotFound();
        }

        var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == id);
        proceso.ProcesosFuncionarios = procesosFuncionarios;
        var procesoVM = new ProcesoDto
        {
            IdProceso = proceso.IdProceso,
            Alertas = proceso.Alerta.ToAlertaDTOCollection(),
            Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection(),
            FechaFin = proceso.FechaFin,
            FechaInicio = proceso.FechaInicio,
            Formularios = proceso.Formularios.ToFormularioDTOCollection(),
            IdEstadoProceso = proceso.IdEstadoProceso,
            EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO(),
            NombreProceso = proceso.NombreProceso,
            Observaciones = proceso.Observaciones,
            Funcionarios = funcionarios.ToFuncionariosCollectionDTO(procesosFuncionarios, proceso.Corporativo.HasValue && proceso.Corporativo.Value),
            Corporativo = proceso.Corporativo.HasValue && proceso.Corporativo.Value
        };
        ViewBag.rutaCumplimiento = _configuration["Application:CumplimientoProceso"] + "&$filter=Procesos/NombreProceso eq '" + procesoVM.NombreProceso + "'";
        ViewBag.rutaAuditoria = _configuration["Application:AuditoriaProceso"] + "&$filter=Procesos/NombreProceso eq '" + procesoVM.NombreProceso + "'";
        var (Result, Errors) = await ValidarProcesoAsync(proceso);
        ViewBag.IsValid = Result;
        ViewBag.Errors = Errors;

        ModelState.Clear();

        return View(procesoVM);
    }

    // POST: administracion/Procesos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string? idTabs, [Bind("IdProceso,IdEstadoProceso,Corporativo,NombreProceso,FechaInicio,FechaFin,Observaciones")] ProcesoDto procesoDTO)
    {
        if (id != procesoDTO.IdProceso)
        {
            return NotFound();
        }

        var proceso = await _procesosRepository.GetByIdAsync(procesoDTO.IdProceso);

        await ValidarNombreEdit(procesoDTO);
        if (proceso == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            proceso.IdEstadoProceso = procesoDTO.IdEstadoProceso;
            proceso.Corporativo = procesoDTO.Corporativo;
            proceso.FechaInicio = procesoDTO.FechaInicio;
            if (procesoDTO.FechaFin == null)
            {
                procesoDTO.FechaFin = DateTime.MinValue;
            }
            proceso.FechaFin = procesoDTO.FechaFin == DateTime.MinValue ? DateTime.MaxValue : procesoDTO.FechaFin.Value;
            proceso.NombreProceso = procesoDTO.NombreProceso;
            proceso.Observaciones = procesoDTO.Observaciones;

            try
            {

                proceso.Formularios = null;
                //   await _logger.LogAsync(proceso.IdProceso, "Proceso:Editar", "Solicitud de inicio de edición de Proceso", "Inicio de edición", TipoAuditoria.Negocio);
                await _procesosRepository.UpdateAsync(proceso);
                await _logger.LogAsync(proceso.IdProceso, "Editar Proceso", proceso.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _logger.LogAsync(procesoDTO.IdProceso, "Editar Proceso", proceso.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Sistema);
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                if (!await _procesosRepository.Exist(proceso.IdProceso))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == id);

        procesoDTO.Alertas = proceso.Alerta.ToAlertaDTOCollection();
        procesoDTO.Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection();
        procesoDTO.Formularios = proceso.Formularios.ToFormularioDTOCollection();
        procesoDTO.EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO();
        procesoDTO.Funcionarios = funcionarios.ToFuncionariosCollectionDTO(procesosFuncionarios, proceso.Corporativo.HasValue && proceso.Corporativo.Value);

        var (Result, Errors) = await ValidarProcesoAsync(procesoDTO.ToProceso());
        ViewBag.IsValid = Result;
        ViewBag.Errors = Errors;

        if (idTabs == "alertas")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "show active";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "disclaimers")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "show active";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";

        }
        else if (idTabs == "formularios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "show active";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "auditoria")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "show active";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "funcionarios")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "show active";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }
        else if (idTabs == "Cumplimiento")
        {
            ViewBag.proceso = "";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "show active";
        }
        else
        {
            ViewBag.proceso = "show active";
            ViewBag.alertas = "";
            ViewBag.disclaimers = "";
            ViewBag.formularios = "";
            ViewBag.Funcionarios = "";
            ViewBag.Auditoria = "";
            ViewBag.Cumplimiento = "";
        }


        return View(procesoDTO);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFuncionarios([FromBody] FuncionariosProcesoAjaxDto funcionariosProceso)
    {
        try
        {

            var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
            ProcesosFuncionarioRepository procesosFuncionarioRepository = (ProcesosFuncionarioRepository)_procesosFuncionarioRepository;
            var funcionariosDelProceso = await procesosFuncionarioRepository.GetAllActiveAsync(pf => pf.IdProceso == funcionariosProceso.IdProceso);
            var agregadosIds = funcionariosProceso.Funcionarios.Where(x => !funcionariosDelProceso.Any(fp => fp.IdFuncionario == x));
            var agregados = funcionarios.Where(f => agregadosIds.Contains(f.IdFuncionario)).ToList();
            await procesosFuncionarioRepository.AddRange(funcionariosProceso.IdProceso, agregados);
            var eliminados = funcionariosDelProceso.Where(fp => !funcionariosProceso.Funcionarios.Any(x => x == fp.IdFuncionario)).ToList();
            await procesosFuncionarioRepository.DeleteRange(eliminados);
            //     await _logger.LogAsync(funcionariosProceso.IdProceso, "Crear Proceso Funcionario", "Solicitud de inicio de asociación de funcionarios a procesos", "Exitoso", TipoAuditoria.Negocio);
        }
        catch (Exception ex)
        {
            // TODO: Log error
            await _logger.LogAsync(funcionariosProceso.IdProceso, "Crear Proceso Funcionario", "Solicitud de inicio de asociación de funcionarios a procesos", $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
            Console.Write(ex.ToString());
            return Json(new { succeed = false });
        }
        return Json(new { succeed = true });
    }

    // GET: administracion/Procesos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var proceso = await _procesosRepository.GetByIdAsync(id);
        if (proceso == null)
        {
            return NotFound();
        }

        var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == id);

        var procesoVM = new ProcesoDto
        {
            IdProceso = proceso.IdProceso,
            Alertas = proceso.Alerta.ToAlertaDTOCollection(),
            Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection(),
            FechaFin = proceso.FechaFin,
            FechaInicio = proceso.FechaInicio,
            Formularios = proceso.Formularios.ToFormularioDTOCollection(),
            IdEstadoProceso = proceso.IdEstadoProceso,
            EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO(),
            NombreProceso = proceso.NombreProceso,
            Observaciones = proceso.Observaciones,
            Funcionarios = funcionarios.ToFuncionariosCollectionDTO(procesosFuncionarios, proceso.Corporativo.HasValue && proceso.Corporativo.Value),
            Corporativo = proceso.Corporativo.HasValue && proceso.Corporativo.Value
        };

        return View(procesoVM);
    }

    // POST: administracion/Procesos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proceso = await _procesosRepository.GetByIdAsync(id);
        if (proceso != null)
        {
            try
            {
                //     await _logger.LogAsync(proceso.IdProceso, "Proceso:Eliminar", "Solicitud de inicio de Eliminación de proceso", "Inicio de Eliminación", TipoAuditoria.Negocio);
                await _procesosRepository.DeleteAsync(proceso);
                await _logger.LogAsync(proceso.IdProceso, "Eliminar Proceso " + proceso.NombreProceso, proceso.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);

            }
            catch (Exception ex)
            {

                await _logger.LogAsync(proceso.IdProceso, "Eliminar Proceso", proceso.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);
            }

        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Configured(int IdProceso)
    {
        var proceso = await _procesosRepository.GetByIdAsync(IdProceso);
        if (proceso == null)
        {
            return NotFound();
        }

        var (Result, Errors) = await ValidarProcesoAsync(proceso);
        if (Result)
        {
            try
            {
                //        await _logger.LogAsync(proceso.IdProceso, "Proceso:Configurado", "Solicitud de inicio de configuración de proceso", "Inicio de configuración", TipoAuditoria.Negocio);
                EstadoProceso configurado = (await _estadoProceso.GetAllActiveAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso == "Configurado");
                proceso.IdEstadoProceso = configurado.IdEstadoProceso;
                proceso.IdEstadoProcesoNavigation = configurado;
                proceso.Formularios = null;
                Proceso procesoInsertado = await _procesosRepository.UpdateAsync(proceso);
                await _logger.LogAsync(proceso.IdProceso, "Configurar Proceso " + procesoInsertado.NombreProceso, procesoInsertado.ToAuditoria(), "Exitoso", TipoAuditoria.Negocio);
            }
            catch (Exception ex)
            {

                await _logger.LogAsync(proceso.IdProceso, "Configurar Proceso ", proceso.ToAuditoria(), $"Fallido: ERROR {ex.Message}", TipoAuditoria.Negocio);
                var config = TelemetryConfiguration.CreateDefault();
                var client = new TelemetryClient(config);
                client.TrackException(ex);
            }
            // Si el proceso es corporativo, asegurarse de que todos los funcionarios se agregan
            if (proceso.Corporativo.HasValue && proceso.Corporativo.Value)
            {
                var funcionarios = await _funcionariosRepository.GetAllActiveAsync();
                var funcionariosProceso = new { proceso.IdProceso, Funcionarios = funcionarios.Select(f => f.IdFuncionario).ToList() };
                ProcesosFuncionarioRepository procesosFuncionarioRepository = (ProcesosFuncionarioRepository)_procesosFuncionarioRepository;
                var funcionariosDelProceso = await procesosFuncionarioRepository.GetAllActiveAsync(pf => pf.IdProceso == funcionariosProceso.IdProceso);
                var agregadosIds = funcionariosProceso.Funcionarios.Where(x => !funcionariosDelProceso.Any(fp => fp.IdFuncionario == x));
                var agregados = funcionarios.Where(f => agregadosIds.Contains(f.IdFuncionario)).ToList();
                await procesosFuncionarioRepository.AddRange(funcionariosProceso.IdProceso, agregados);
            }
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Edit), new { Id = IdProceso });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CierreAnticipado(int IdProceso)
    {
        var proceso = await _procesosRepository.GetByIdAsync(IdProceso);
        if (proceso == null)
        {
            return NotFound();
        }

        if (proceso.IdEstadoProcesoNavigation.NombreEstadoProceso == "En ejecución")
        {
            var estadoProceso = (await _estadoProceso.GetAllActiveAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso == "Cerrado anticipado");
            int idCerradoAnticipado = estadoProceso?.IdEstadoProceso ?? 5;
            proceso.IdEstadoProceso = idCerradoAnticipado;
            proceso.IdEstadoProcesoNavigation = estadoProceso;
            proceso.FechaCierre = DateTime.UtcNow;
            proceso.Formularios = null;
            var procesoInse = await _procesosRepository.UpdateAsync(proceso);
            await _logger.LogAsync(IdProceso, "Cierre Anticipado Proces " + procesoInse.NombreProceso, "Se realizó el cierre anticipado correctamente del proceso " + proceso.ToAuditoria(), "Exitoso", TipoAuditoria.Sistema);
            return RedirectToAction("Index");
        }

        await _logger.LogAsync(IdProceso, "Cierre Anticipado Proceso", $"Se intentó Cierre anticipado y el estado es {proceso.IdEstadoProcesoNavigation.NombreEstadoProceso}", "Fallido", TipoAuditoria.Sistema);

        return View("Error", new ErrorViewModel { ErrorMsg = "El proceso no se encuentra en el estado correcto" });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReabrirProceso(int IdProceso, [Bind("IdProceso,NombreProceso,FechaInicio,FechaFin")] ProcesoDto procesoDTO)
    {
        var proceso = await _procesosRepository.GetByIdAsync(IdProceso);


    
        if (proceso == null)
        {
            return NotFound();
        }

        proceso.FechaInicio = procesoDTO.FechaInicio;


        if (procesoDTO.FechaFin==null)
        {
            procesoDTO.FechaFin = DateTime.MinValue;
        }

        proceso.FechaFin    = procesoDTO.FechaFin.Value == DateTime.MinValue ? DateTime.MaxValue : procesoDTO.FechaFin.Value;

   

        foreach (var item in proceso.Alerta)
        {
            if (item.Diligenciado == false)
            {
                item.Fecha = procesoDTO.FechaInicio;
            }
        }
        var isvalid = await ValidarProcesoReabrirAsync(proceso);
        if (isvalid.Result)
        {
            var estadoProceso = (await _estadoProceso.GetAllActiveAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso == "En ejecución");
            proceso.IdEstadoProceso = estadoProceso?.IdEstadoProceso ?? 3;
            proceso.IdEstadoProcesoNavigation = estadoProceso;
            proceso.Formularios = null;
            proceso.FechaCierre = null;
            var procesoInse = await _procesosRepository.UpdateAsync(proceso);
            await _logger.LogAsync(IdProceso, "Reabrir Proceso " + procesoInse.NombreProceso, "Se realizó la reapertura correctamente del proceso " + proceso.ToAuditoria(), "Exitoso", TipoAuditoria.Sistema);
            return RedirectToAction("Index");
        }
       
       //await _logger.LogAsync(IdProceso, "Cierre Anticipado Proceso", $"Se intentó Cierre anticipado y el estado es {proceso.IdEstadoProcesoNavigation.NombreEstadoProceso}", "Fallido", TipoAuditoria.Sistema);

        var errores = "Error al reabrir el proceso:<br><br>";

        for (int i=0; i < isvalid.Errors.Count; i++)
        {
            errores += "-"+isvalid.Errors[i] + "<br>";
        }

        return View("Error", new ErrorViewModel { ErrorMsg = errores });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarFecha(int IdProceso, [Bind("IdProceso,NombreProceso,FechaInicio,FechaFin")] ProcesoDto procesoDTO)
    {
        var proceso = await _procesosRepository.GetByIdAsync(IdProceso);



        if (proceso == null)
        {
            return NotFound();
        }

       

        if (procesoDTO.FechaFin == null)
        {
            procesoDTO.FechaFin = DateTime.MinValue;
        }

        proceso.FechaFin = procesoDTO.FechaFin.Value == DateTime.MinValue ? DateTime.MaxValue : procesoDTO.FechaFin.Value;



     
        var isvalid = await ValidarProcesoReabrirAsync(proceso);
        if (isvalid.Result)
        {
    
            proceso.Formularios = null;
            proceso.FechaCierre = null;
            var procesoInse = await _procesosRepository.UpdateAsync(proceso);
            await _logger.LogAsync(IdProceso, "Reabrir Proceso " + procesoInse.NombreProceso, "Se realizó la reapertura correctamente del proceso " + proceso.ToAuditoria(), "Exitoso", TipoAuditoria.Sistema);
            return RedirectToAction("Index");
        }

        //await _logger.LogAsync(IdProceso, "Cierre Anticipado Proceso", $"Se intentó Cierre anticipado y el estado es {proceso.IdEstadoProcesoNavigation.NombreEstadoProceso}", "Fallido", TipoAuditoria.Sistema);

        var errores = "Error al reabrir el proceso:<br><br>";

        for (int i = 0; i < isvalid.Errors.Count; i++)
        {
            errores += "-" + isvalid.Errors[i] + "<br>";
        }

        return View("Error", new ErrorViewModel { ErrorMsg = errores });
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cierre(int IdProceso)
    {
        var proceso = await _procesosRepository.GetByIdAsync(IdProceso);
        if (proceso == null)
        {
            return NotFound();
        }

        if (proceso.IdEstadoProcesoNavigation.NombreEstadoProceso == "En ejecución")
        {
            int idCerrado = (await _estadoProceso.GetAllActiveAsync()).FirstOrDefault(ep => ep.NombreEstadoProceso == "Cerrado")?.IdEstadoProceso ?? 5;
            proceso.IdEstadoProceso = idCerrado;
            proceso.Formularios = null;
            var procesoInse = await _procesosRepository.UpdateAsync(proceso);
            await _logger.LogAsync(IdProceso, "Cierre Anticipado Proceso  " + procesoInse.NombreProceso, "Se realizó el cierre correctamente", "Exitoso", TipoAuditoria.Sistema);
            return RedirectToAction("Index");
        }

        await _logger.LogAsync(IdProceso, "Cierre Anticipado Proceso", $"Se intentó Cierre y el estado es {proceso.IdEstadoProcesoNavigation.NombreEstadoProceso}", "Fallido", TipoAuditoria.Sistema);

        return View("Error", new ErrorViewModel { ErrorMsg = "El proceso no se encuentra en el estado correcto" });
    }

    private async Task<(bool Result, List<string> Errors)> ValidarProcesoAsync(Proceso proceso)
    {
        bool result = true;
        List<string> errors = new();
        DateOnly startDate = DateOnly.FromDateTime(proceso.FechaInicio);
        DateOnly endDate = DateOnly.FromDateTime(proceso.FechaFin);
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        if (startDate < now)
        {
            result = false;
            errors.Add("La fecha de inicio es menor al día de hoy");
        }

        // Validar que contiene alertas y al menos una alerta no diligenciado
        if (proceso.Alerta.Count == 0 || !proceso.Alerta.Any(a => !a.Diligenciado))
        {
            result = false;
            errors.Add("No hay al menos una alerta no diligenciada configurada");
        }
        // Validar que contiene al menos un aviso de diligenciado
        if (!proceso.Alerta.Any(a => a.Diligenciado))
        {
            result = false;
            errors.Add("No hay una alerta para la declaración diligenciada");
        }
        // Validar que las alertas
        if (proceso.Alerta.Any(a => !a.Diligenciado && (DateOnly.FromDateTime(a.Fecha) < startDate || DateOnly.FromDateTime(a.Fecha) > endDate)))
        {
            result = false;
            errors.Add("Existe al menos una alerta con una fecha inválida");
        }
        // Validar que contiene formularios
        if (proceso.Formularios.Count == 0)
        {
            result = false;
            errors.Add("No hay formularios asociados");
        }

        // Validar que contiene funcionarios
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == proceso.IdProceso);
        if (proceso.Corporativo.HasValue && proceso.Corporativo.Value && (await _funcionariosRepository.GetAllActiveAsync()).Count == 0)
        {
            result = false;
            errors.Add("El proceso es corporativo pero no se han creado funcionarios");
        }
        else if (proceso.Corporativo.HasValue && !proceso.Corporativo.Value && procesosFuncionarios.Count == 0)
        {
            result = false;
            errors.Add("No hay funcionarios asociados y el proceso no es corporativo");
        }
        var procesosActivos = await _procesosRepository.GetAllActiveAsync();

        // Validar procesos solapados
        //Validar proceso solapado con los mismos formularios
        var procesosSolapados = procesosActivos
            .Where(p =>
            (p.IdEstadoProceso == 2 || p.IdEstadoProceso == 3) &&
            (p.FechaInicio < proceso.FechaFin && proceso.FechaInicio < p.FechaFin)
            );
        var formulariosProcesosSolapados = procesosSolapados.SelectMany(p => p.Formularios).Select(f => new { f.IdTipoDeclaracion, f.IdProceso, f.IdProcesoNavigation.NombreProceso, f.IdProcesoNavigation.IdEstadoProcesoNavigation.NombreEstadoProceso, f.IdTipoDeclaracionNavigation?.NombreDeclaracion });

        var idFormularios = proceso.Formularios.Where(f => f.IdTipoDeclaracion != 5).Select(f => f.IdTipoDeclaracion );
        foreach (var idFormulario in formulariosProcesosSolapados.Where(fps => idFormularios.Contains(fps.IdTipoDeclaracion)))
        {
            result = false;
            errors.Add($"El proceso <strong>{idFormulario.NombreProceso}</strong> en estado <strong>{idFormulario.NombreEstadoProceso}</strong>, incluye el formulario <strong>{idFormulario.NombreDeclaracion}</strong> en fechas que se solapan");
        }

        return (result, errors);
    }

    private async Task<(bool Result, List<string> Errors)> ValidarProcesoTemplateAsync(ProcesoDto proceso)
    {
        bool result = true;
        List<string> errors = new();
        DateOnly startDate = DateOnly.FromDateTime(proceso.FechaInicio);
        if (proceso.FechaFin == default(DateTime) || proceso.FechaFin == null)
        {
            proceso.FechaFin = DateTime.MaxValue;

        }

        DateOnly endDate = DateOnly.FromDateTime(proceso.FechaFin == DateTime.MinValue ? DateTime.MaxValue : proceso.FechaFin.Value);
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        if (startDate < now)
        {
            result = false;
            errors.Add("La fecha de inicio es menor al día de hoy");
        }

        // Validar que contiene alertas y al menos una alerta no diligenciado
        if (proceso.Alertas.Count == 0 || !proceso.Alertas.Any(a => !a.Diligenciado))
        {
            result = false;
            errors.Add("No hay al menos una alerta no diligenciada configurada");
        }
        // Validar que contiene al menos un aviso de diligenciado
        if (!proceso.Alertas.Any(a => a.Diligenciado))
        {
            result = false;
            errors.Add("No hay una alerta para la declaración diligenciada");
        }

        // Validar que contiene formularios
        if (proceso.Formularios.Count == 0)
        {
            result = false;
            errors.Add("No hay formularios asociados");
        }

        // Validar que contiene funcionarios
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == proceso.IdProceso);
        if (proceso.Corporativo && proceso.Corporativo && (await _funcionariosRepository.GetAllActiveAsync()).Count == 0)
        {
            result = false;
            errors.Add("El proceso es corporativo pero no se han creado funcionarios");
        }
        else if (proceso.Corporativo && !proceso.Corporativo && procesosFuncionarios.Count == 0)
        {
            result = false;
            errors.Add("No hay funcionarios asociados y el proceso no es corporativo");
        }
        var procesosActivos = await _procesosRepository.GetAllActiveAsync();

        // Validar procesos solapados
        //Validar proceso solapado con los mismos formularios
        var procesosSolapados = procesosActivos
            .Where(p =>
            (p.IdEstadoProceso == 2 || p.IdEstadoProceso == 3) &&
            (p.FechaInicio <= proceso.FechaFin && proceso.FechaInicio <= p.FechaFin)
            );
        var formulariosProcesosSolapados = procesosSolapados.SelectMany(p => p.Formularios).Select(f => new { f.IdTipoDeclaracion, f.IdProceso, f.IdProcesoNavigation.NombreProceso, f.IdProcesoNavigation.IdEstadoProcesoNavigation.NombreEstadoProceso, f.IdTipoDeclaracionNavigation?.NombreDeclaracion });

        var idFormularios = proceso.Formularios.Where(f => f.IdTipoDeclaracion != 5).Select(f => f.IdTipoDeclaracion);
        foreach (var idFormulario in formulariosProcesosSolapados.Where(fps => idFormularios.Contains(fps.IdTipoDeclaracion)))
        {
            result = false;
            errors.Add($"El proceso {idFormulario.NombreProceso} en estado {idFormulario.NombreEstadoProceso}, incluye el formulario {idFormulario.NombreDeclaracion} en fechas que se solapan");
        }

        return (result, errors);
    }
    private async Task<(bool Result, List<string> Errors)> ValidarProcesoReabrirAsync(Proceso proceso)
    {
        bool result = true;
        List<string> errors = new();
        DateOnly startDate = DateOnly.FromDateTime(proceso.FechaInicio);
        DateOnly endDate = DateOnly.FromDateTime(proceso.FechaFin);
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);



        // Validar que contiene alertas y al menos una alerta no diligenciado
        if (proceso.Alerta.Count == 0 || !proceso.Alerta.Any(a => !a.Diligenciado))
        {
            result = false;
            errors.Add("No hay al menos una alerta no diligenciada configurada");
        }
        // Validar que contiene al menos un aviso de diligenciado
        if (!proceso.Alerta.Any(a => a.Diligenciado))
        {
            result = false;
            errors.Add("No hay una alerta para la declaración diligenciada");
        }
        // Validar que las alertas
        if (proceso.Alerta.Any(a => !a.Diligenciado && (DateOnly.FromDateTime(a.Fecha) < startDate || DateOnly.FromDateTime(a.Fecha) > endDate)))
        {
            result = false;
            errors.Add("Existe al menos una alerta con una fecha inválida");
        }
        // Validar que contiene formularios
        if (proceso.Formularios.Count == 0)
        {
            result = false;
            errors.Add("No hay formularios asociados");
        }

        // Validar que contiene funcionarios
        var procesosFuncionarios = await ((ProcesosFuncionarioRepository)_procesosFuncionarioRepository).GetAllActiveAsync(pf => pf.IdProceso == proceso.IdProceso);
        if (proceso.Corporativo.HasValue && proceso.Corporativo.Value && (await _funcionariosRepository.GetAllActiveAsync()).Count == 0)
        {
            result = false;
            errors.Add("El proceso es corporativo pero no se han creado funcionarios");
        }
        else if (proceso.Corporativo.HasValue && !proceso.Corporativo.Value && procesosFuncionarios.Count == 0)
        {
            result = false;
            errors.Add("No hay funcionarios asociados y el proceso no es corporativo");
        }
        var procesosActivos = await _procesosRepository.GetAllActiveAsync();

        // Validar procesos solapados
        //Validar proceso solapado con los mismos formularios
        var procesosSolapados = procesosActivos
            .Where(p =>
            (p.IdEstadoProceso == 2 || p.IdEstadoProceso == 3) &&
            (p.FechaInicio < proceso.FechaFin && proceso.FechaInicio < p.FechaFin)
            );
        var formulariosProcesosSolapados = procesosSolapados.SelectMany(p => p.Formularios).Select(f => new { f.IdTipoDeclaracion, f.IdProceso, f.IdProcesoNavigation.NombreProceso, f.IdProcesoNavigation.IdEstadoProcesoNavigation.NombreEstadoProceso, f.IdTipoDeclaracionNavigation?.NombreDeclaracion });

        var idFormularios = proceso.Formularios.Where(f => f.IdTipoDeclaracion != 5 && f.IdProceso != proceso.IdProceso).Select(f => f.IdTipoDeclaracion);
        foreach (var idFormulario in formulariosProcesosSolapados.Where(fps => idFormularios.Contains(fps.IdTipoDeclaracion)))
        {
            result = false;
            errors.Add($"El proceso <strong>{idFormulario.NombreProceso}</strong> en estado <strong>{idFormulario.NombreEstadoProceso}</strong>, incluye el formulario <strong>{idFormulario.NombreDeclaracion}</strong> en fechas que se solapan");
        }

        return (result, errors);
    }
}
