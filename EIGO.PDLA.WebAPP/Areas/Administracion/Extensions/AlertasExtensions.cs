using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
public static class AlertasExtensions
{
    public static AlertaDto ToAlertaDTO(this Alerta alerta)
    {
        return new AlertaDto
        {
            Asunto = alerta.Asunto,
            AvisoConfidencialidad = alerta.AvisoConfidencialidad,
            Cuerpo = alerta.Cuerpo,
            Diligenciado = alerta.Diligenciado,
            Estatus = alerta.Estatus,
            Fecha = alerta.Fecha,
            IdAlerta = alerta.IdAlerta,
            IdProceso = alerta.IdProceso,
            SubTitulo = alerta.SubTitulo,
            Titulo = alerta.Titulo,
            FechaFinProceso = alerta.IdProcesoNavigation.FechaFin,
            FechaInicioProceso = alerta.IdProcesoNavigation.FechaInicio
        };
    }

    public static ICollection<AlertaDto> ToAlertaDTOCollection(this ICollection<Alerta> alertas)
    {
        return alertas.Select(alerta => alerta.ToAlertaDTO()).ToList();
    }

    public static Alerta ToAlerta(this AlertaDto alertaDTO)
    {
        return new Alerta
        {
            IdProceso = alertaDTO.IdProceso,
            IdAlerta = alertaDTO.IdAlerta,
            Asunto = alertaDTO.Asunto,
            Titulo = alertaDTO.Titulo,
            AvisoConfidencialidad = alertaDTO.AvisoConfidencialidad,
            Estatus = alertaDTO.Estatus,
            Cuerpo = alertaDTO.Cuerpo,
            Diligenciado = alertaDTO.Diligenciado,
            Fecha = alertaDTO.Fecha,
            SubTitulo = alertaDTO.SubTitulo
        };
    }
    public static string ToAuditoria(this AlertaDto alertaDTO)
    {
        if (alertaDTO == null)
        {
            return string.Empty;
        }
        StringBuilder sb = new();
     
        sb.AppendLine($"Titulo: {alertaDTO.Titulo}");
        sb.AppendLine($"Asunto: {alertaDTO.Asunto}");
        sb.AppendLine($"AvisoConfidencialidad: {alertaDTO.AvisoConfidencialidad}");
        sb.AppendLine($"Estatus: {alertaDTO.Estatus}");
        sb.AppendLine($"Cuerpo: {alertaDTO.Cuerpo}");
        sb.AppendLine($"Diligenciado: {alertaDTO.Diligenciado}");
        sb.AppendLine($"Fecha: {alertaDTO.Fecha}");
        sb.AppendLine($"SubTitulo: {alertaDTO.SubTitulo}");
        return sb.ToString();
    }
}
