using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EIGO.PDLA.Tests.WebApp.Areas.Administracion.Controllers;
public class ProcesosControllerTests
{
    private readonly HttpContext httpContext = new DefaultHttpContext
    {
        TraceIdentifier = "Test"
    };

    private readonly Mock<IHttpContextAccessor> httpContextAccessor = new();
    private readonly DbContextOptions<DeclaracionesContext> options = new DbContextOptionsBuilder<DeclaracionesContext>()
            .UseInMemoryDatabase(databaseName: "DeclaracionesProcessControllerTestsDb")
            .Options;
    private IConfiguration _configuration;
    private IEntityRepository<Alerta> _alertaRepository;
    private IEntityRepository<Disclaimer> _disclaimerRepository;
    private IEntityRepository<Proceso> _procesosRepository;
    private IEntityRepository<EstadoProceso> _estadoProcesoRepository;
    private IEntityRepository<Formulario> _formularioRepository;
    private IEntityRepository<Funcionario> _funcionarioRepository;
    private IEntityRepository<ProcesosFuncionario> _procesosFuncionarioRepository;
    private IEntityRepository<ProcesoDisclaimerFormulario> _procesoDisclaimerFormularioRepository;
    private IPdlaLogger _pdlaLogger;

    private DeclaracionesContext context;

    [OneTimeSetUp]
    public void Setup()
    {
        context = new DeclaracionesContext(httpContextAccessor.Object, options);
        context.EstadoProcesos.Add(new() { NombreEstadoProceso = "Por configurar" });
        context.EstadoProcesos.Add(new() { NombreEstadoProceso = "Configurado" });
        context.EstadoProcesos.Add(new() { NombreEstadoProceso = "En ejecución" });
        context.EstadoProcesos.Add(new() { NombreEstadoProceso = "Cerrado" });
        context.EstadoProcesos.Add(new() { NombreEstadoProceso = "Cerrado anticipado" });
        context.SaveChanges();

        _alertaRepository = new AlertaRepository(context);
        _disclaimerRepository = new DisclaimersRepository(context);
        _estadoProcesoRepository = new EstadoProcesoRepository(context);
        _formularioRepository = new FormularioRepository(context);
        _funcionarioRepository = new FuncionarioRepository(context);
        _procesosRepository = new ProcesoRepository(context);
        _procesosFuncionarioRepository = new ProcesosFuncionarioRepository(context, httpContextAccessor.Object);
        _procesoDisclaimerFormularioRepository = new ProcesoDisclaimerFormularioRepository(context);
        _pdlaLogger = new PdlaLogger(context, httpContextAccessor.Object);
        var configuration = new Dictionary<string, string>
        {
            {"Application:CumplimientoProceso", ""},
            {"Application:AuditoriaProceso", ""}
        };
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(configuration).Build();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        context.Dispose();
    }

    [Test, Order(1)]
    public async Task ProcesosControllerNoDataIndexAsync()
    {
        ProcesosController procesosController = new(
              _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        var index = await procesosController.Index();
        Assert.IsNotNull(index);
        Assert.IsInstanceOf<ViewResult>(index);

        var result = index as ViewResult;
        Assert.AreEqual(null, result?.StatusCode);

        Assert.IsInstanceOf<ICollection<ProcesoDto>>(result?.ViewData.Model);
        var model = result?.ViewData.Model as ICollection<ProcesoDto>;
        Assert.AreEqual(0, model?.Count);
    }

    [Test, Order(3)]
    public async Task ProcesosControllerWithDataIndexAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        var index = await procesosController.Index();
        Assert.IsNotNull(index);
        Assert.IsInstanceOf<ViewResult>(index);

        var result = index as ViewResult;
        Assert.AreEqual(null, result?.StatusCode);

        Assert.IsInstanceOf<ICollection<ProcesoDto>>(result?.ViewData.Model);
        var model = result?.ViewData.Model as ICollection<ProcesoDto>;
        Assert.AreEqual(1, model?.Count);
    }

    [Test, Order(4)]
    public void ProcesosControllerCreateAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        var create = procesosController.Create();
        Assert.IsNotNull(create);
        Assert.IsInstanceOf<ViewResult>(create);

        var result = create as ViewResult;
        Assert.AreEqual(null, result?.StatusCode);

        Assert.IsNull(result?.ViewData.Model);
    }

    [Test, Order(5)]
    public async Task ProcesosControllerCreateNoDataAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        ProcesoDto procesoDTO = new()
        {
            NombreProceso = "Proceso"
        };

        procesosController.ModelState.AddModelError("N", "La fecha es requerida");
        var create = await procesosController.Create(procesoDTO);
        Assert.IsNotNull(create);
        Assert.IsInstanceOf<ViewResult>(create);

        var result = create as ViewResult;
        var model = result?.Model;
        Assert.IsNotNull(model);
        Assert.AreEqual(procesoDTO, model);
    }

    [Test, Order(2)]
    public async Task ProcesosControllerCreateWithDataAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        ProcesoDto procesoDTO = new()
        {
            Alertas = new List<AlertaDto>(),
            Disclaimers = new List<DisclaimerDTO>(),
            EstadoProceso = new EstadoProcesoDTO(),
            Eliminado = false,
            FechaFin = new DateTime(2000, 12, 1, 0, 0, 0, DateTimeKind.Utc),
            FechaInicio = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Formularios = new List<FormularioDTO>(),
            IdEstadoProceso = 1,
            IdProceso = 0,
            NombreProceso = "Proceso de prueba",
            Observaciones = "Observaciones"
        };

        var create = await procesosController.Create(procesoDTO);
        Assert.IsNotNull(create);
        Assert.IsInstanceOf<RedirectToActionResult>(create);

        var result = create as RedirectToActionResult;
        Assert.AreEqual("Index", result?.ActionName);
    }

    [Test, Order(6)]
    public async Task ProcesosControllerCierreAnticipadoAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        var p = context.Procesos.Find(1);
        p.IdEstadoProceso = 3;
        context.Procesos.Update(p);
        context.SaveChanges();

        var cerradoAnticipado = await procesosController.CierreAnticipado(1);
        Assert.IsNotNull(cerradoAnticipado);
        Assert.IsInstanceOf<RedirectToActionResult>(cerradoAnticipado);

        var result = cerradoAnticipado as RedirectToActionResult;
        Assert.AreEqual("Index", result?.ActionName);
    }

    [Test, Order(7)]
    public async Task ProcesosControllerCierreAsync()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };

        var p = context.Procesos.Find(1);
        p.IdEstadoProceso = 3;
        context.Procesos.Update(p);
        context.SaveChanges();

        var cerradoAnticipado = await procesosController.Cierre(1);
        Assert.IsNotNull(cerradoAnticipado);
        Assert.IsInstanceOf<RedirectToActionResult>(cerradoAnticipado);

        var result = cerradoAnticipado as RedirectToActionResult;
        Assert.AreEqual("Index", result?.ActionName);
    }

    [Test, Order(8)]
    public async Task ProcesosControllerCreateAlertaInvalidDates()
    {
        ProcesosController procesosController = new(
                  _configuration,
            _alertaRepository,
            _disclaimerRepository,
            _estadoProcesoRepository,
            _formularioRepository,
            _funcionarioRepository,
            _procesosRepository,
            _procesosFuncionarioRepository,
            _procesoDisclaimerFormularioRepository,
            _pdlaLogger)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            }
        };
        //FechaFin = new DateTime(2000, 12, 1, 0, 0, 0, DateTimeKind.Utc),
        //FechaInicio = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        context.Alertas.Add(new Alerta
        {
            Asunto = "Asunto",
            AvisoConfidencialidad = "Aviso de confidencialidad",
            Cuerpo = "Cuerpo",
            IdProceso = 1,
            Fecha = new DateTime(1999, 12, 31, 11, 59, 59, DateTimeKind.Utc),
            Titulo = "Titulo"
        });
        context.SaveChanges();
        var procesos = await procesosController.Details(1, "");
        Assert.IsInstanceOf<ViewResult>(procesos);
        ViewResult viewResult = procesos as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.IsTrue(viewResult.ViewData.ContainsKey("Errors"));
        List<string> errors = viewResult.ViewData["Errors"] as List<string>;
        Assert.IsTrue(errors.Any(x => x == "Existe al menos una alerta con una fecha inválida"));
    }
}
