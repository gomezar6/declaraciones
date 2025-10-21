using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.Tests.WebApp;

[TestFixture]
public class DTOsTest
{
    [Test]
    public void AlertaDTOsTests()
    {
        AlertaDto alertaDTO = new()
        {
            Asunto = "Asunto",
            AvisoConfidencialidad = "Aviso de confidencialidad",
            IdAlerta = 1,
            Cuerpo = "Cuerpo",
            Estatus = true,
            Fecha = new DateTime(2022, 2, 1, 0, 0, 0),
            FechaFinProceso = new DateTime(2022, 3, 1, 0, 0, 0),
            FechaInicioProceso = new DateTime(2022, 1, 1, 0, 0, 0),
            IdProceso = 2,
            SubTitulo = "Sub título",
            Titulo = "Título"
        };

        Assert.NotNull(alertaDTO);
        Assert.AreEqual("Asunto", alertaDTO.Asunto);
        Assert.AreEqual("Aviso de confidencialidad", alertaDTO.AvisoConfidencialidad);
        Assert.AreEqual(1, alertaDTO.IdAlerta);
        Assert.AreEqual("Cuerpo", alertaDTO.Cuerpo);
        Assert.IsTrue(alertaDTO.Estatus);
        Assert.AreEqual(new DateTime(2022, 2, 1, 0, 0, 0), alertaDTO.Fecha);
        Assert.AreEqual(new DateTime(2022, 3, 1, 0, 0, 0), alertaDTO.FechaFinProceso);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), alertaDTO.FechaInicioProceso);
        Assert.AreEqual(2, alertaDTO.IdProceso);
        Assert.AreEqual("Sub título", alertaDTO.SubTitulo);
        Assert.AreEqual("Título", alertaDTO.Titulo);

        var builder = WebApplication.CreateBuilder();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.TryAddScoped<IEntityRepository<Proceso>, ProcesoRepository>();
        builder.Services.AddDbContext<DeclaracionesContext>(
            options => options.UseInMemoryDatabase(databaseName: "DeclaracionesRepositoriesTestsDb")
                );

        var vc = new ValidationContext(alertaDTO, builder.Services.BuildServiceProvider(), null);
        List<ValidationResult> results = new();
        Assert.IsTrue(Validator.TryValidateObject(alertaDTO, vc, results, true));

        alertaDTO.Fecha = new DateTime(2021, 12, 31, 23, 59, 59);
        Assert.IsFalse(Validator.TryValidateObject(alertaDTO, vc, results, true));
        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("La fecha es menor a la fecha de inicio del proceso", results[0].ErrorMessage);

        results = new();
        alertaDTO.Fecha = new DateTime(2022, 3, 2, 0, 0, 0);
        Assert.IsFalse(Validator.TryValidateObject(alertaDTO, vc, results, true));
        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("La fecha es mayor a la fecha de fin del proceso", results[0].ErrorMessage);
    }

    [Test]
    public void DeclaracionDTOsTests()
    {
        DeclaracionDTO declaracionDTO = new()
        {
            //  Ciudad = "Ciudad",
            ConfirmacionResponsabilidad = true,
            FechaDeclaracion = new DateTime(2022, 1, 1, 0, 0, 0),
            IdDeclaracion = 1,
            IdEstadoDeclaracion = 2,
            //IdFormulario = 3,
            // IdParticipacion = 4,
            //Nacionalidad = 5,
            //NombreDeclaracion = "Nombre declaracion",
            RecibidaEnFisico = true,
            // Referencia = "Referencia"
        };

        Assert.NotNull(declaracionDTO);
        ///  Assert.AreEqual("Ciudad", declaracionDTO.Ciudad);
        Assert.IsTrue(declaracionDTO.ConfirmacionResponsabilidad);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), declaracionDTO.FechaDeclaracion);
        Assert.AreEqual(1, declaracionDTO.IdDeclaracion);
        Assert.AreEqual(2, declaracionDTO.IdEstadoDeclaracion);
        //Assert.AreEqual(3, declaracionDTO.IdFormulario);
        // Assert.AreEqual(4, declaracionDTO.IdParticipacion);
        //Assert.AreEqual(5, declaracionDTO.Nacionalidad);
        // Assert.AreEqual("Nombre declaracion", declaracionDTO.NombreDeclaracion);
        Assert.IsTrue(declaracionDTO.RecibidaEnFisico);
        //Assert.AreEqual("Referencia", declaracionDTO.Referencia);

        var vc = new ValidationContext(declaracionDTO);
        List<ValidationResult> results = new();
        Assert.IsTrue(Validator.TryValidateObject(declaracionDTO, vc, results, true));
    }
}
