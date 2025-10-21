using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EIGO.PDLA.Tests.WebApp.Areas.Administracion.DTO;

[TestFixture]
public class ProcesoDTOTests
{
    [Test]
    public void ProcesoDTO()
    {
        var dto = new ProcesoDto();
        Assert.IsNotNull(dto);

        dto.Alertas = new List<AlertaDto>
        {
            new AlertaDto
            {
                Asunto = "Asunto",
                AvisoConfidencialidad = "Aviso",
                IdAlerta = 1,
                Cuerpo = "Cuerpo",
                Estatus = true,
                Fecha = new DateTime(),
                IdProceso = 1,
                SubTitulo = "Sub título",
                Titulo = "Título"
            }
        };

        dto.Disclaimers = new List<DisclaimerDTO>
        {
            new DisclaimerDTO {
                IdDisclaimer = 1,
                IdProceso =1,
                Texto="Text",
                Titulo ="Titulo"
            }
        };

        dto.Eliminado = false;
        dto.EstadoProceso = new EstadoProcesoDTO
        {
            IdEstadoProceso = 1,
            NombreEstadoProceso = "Estado proceso"
        };
        dto.FechaFin = new DateTime();
        dto.FechaInicio = new DateTime();
        dto.Formularios = new List<FormularioDTO>
        {
            new FormularioDTO {
                Encabezado = "Encabezado",
                IdFormulario = 1,
                IdProceso = 1,
                PiePagina="Pie de página",
                Texto1 = "Texto 1",
                Texto2 = "Texto 2",
                Texto3 = "Texto 3",
                Texto4 = "Texto 4",
                Titulo = "Título",
                VersionFormulario = 1
            }
        };
        dto.IdEstadoProceso = 1;
        dto.IdProceso = 1;
        dto.NombreProceso = "Nombre proceso";
        dto.Observaciones = "Observaciones";

        Assert.AreEqual(1, dto.Alertas.Count);
        Assert.AreEqual(1, dto.Disclaimers.Count);
        Assert.AreEqual(1, dto.Formularios.Count);
        Assert.AreEqual(1, dto.EstadoProceso.IdEstadoProceso);
    }
}
