using EIGO.PDLA.Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EIGO.PDLA.Tests.Common;

[TestFixture]
public class ProcesosTests
{
    [Test]
    public void ProcesosTest()
    {
        bool corporativo = true;
        bool eliminado = true;
        DateTime fechaDeCierre = new(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime fechaFin = new(2020, 3, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime fechaInicio = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int idEstadoProceso = 1;
        int idProceso = 2;
        string nombreProceso = "Nombre del proceso";
        string observaciones = "Observaciones";
        DateTime creado = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        string creadoPor = "Creado Por";
        DateTime modificado = new(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        string modificadoPor = "Modificado Por";

        Proceso proceso = new()
        {
            Alerta = new List<Alerta>(),
            Corporativo = corporativo,
            Creado = creado,
            CreadoPor = creadoPor,
            Disclaimers = new List<Disclaimer>(),
            Eliminado = eliminado,
            FechaCierre = fechaDeCierre,
            FechaFin = fechaFin,
            FechaInicio = fechaInicio,
            Formularios = new List<Formulario>(),
            IdEstadoProceso = idEstadoProceso,
            IdEstadoProcesoNavigation = new EstadoProceso(),
            IdProceso = idProceso,
            Modificado = modificado,
            ModificadoPor = modificadoPor,
            NombreProceso = nombreProceso,
            Observaciones = observaciones,
            ProcesosFuncionarios = new List<ProcesosFuncionario>()
        };

        Assert.IsNotNull(proceso);
        Assert.AreEqual(typeof(List<Alerta>), proceso.Alerta.GetType());
        Assert.AreEqual(corporativo, proceso.Corporativo);
        Assert.AreEqual(creado, proceso.Creado);
        Assert.AreEqual(creadoPor, proceso.CreadoPor);
        Assert.AreEqual(typeof(List<Disclaimer>), proceso.Disclaimers.GetType());
        Assert.AreEqual(eliminado, proceso.Eliminado);
        Assert.AreEqual(fechaDeCierre, proceso.FechaCierre);
        Assert.AreEqual(fechaFin, proceso.FechaFin);
        Assert.AreEqual(fechaInicio, proceso.FechaInicio);
    }
}
