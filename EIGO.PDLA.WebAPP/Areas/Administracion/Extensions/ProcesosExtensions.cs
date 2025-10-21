using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class ProcesosExtensions
    {
        public static ProcesoDto ToProcesoDTO(this Proceso proceso)
        {
            return new ProcesoDto
            {
                Alertas = proceso.Alerta.ToAlertaDTOCollection(),
                Disclaimers = proceso.Disclaimers.ToDisclaimerDTOCollection(),
                EstadoProceso = proceso.IdEstadoProcesoNavigation.ToEstadoProcesoDTO(),
                Eliminado = proceso.Eliminado,
                FechaFin = proceso.FechaFin,
                FechaInicio = proceso.FechaInicio,
                Formularios = proceso.Formularios.ToFormularioDTOCollection(),
                IdEstadoProceso = proceso.IdEstadoProceso,
                IdProceso = proceso.IdProceso,
                NombreProceso = proceso.NombreProceso,
                Observaciones = proceso.Observaciones,
                Corporativo = proceso.Corporativo.HasValue && proceso.Corporativo.Value,
                Responsable = proceso.CreadoPor
            };
        }

        public static ICollection<ProcesoDto> ToProcesoDTOCollection(this ICollection<Proceso> procesos)
        {
            return procesos.Select(proceso => proceso.ToProcesoDTO()).ToList();
        }

        public static Proceso ToProceso(this ProcesoDto procesoDTO)
        {
            return new Proceso
            {
                IdProceso = procesoDTO.IdProceso,
                IdEstadoProceso = procesoDTO.IdEstadoProceso,
                Eliminado = procesoDTO.Eliminado,
                NombreProceso = procesoDTO.NombreProceso,
                FechaFin = procesoDTO.FechaFin == DateTime.MinValue ? DateTime.MaxValue : procesoDTO.FechaFin.Value,
                FechaInicio = procesoDTO.FechaInicio,
                Observaciones = procesoDTO.Observaciones,
                Corporativo = procesoDTO.Corporativo
            };
        }

        public static string ToAuditoria(this Proceso proceso)
        {
            if (proceso == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();

            sb.AppendLine($"Nombre del Proceso: {proceso.NombreProceso}");
            sb.AppendLine($"Fecha Fin: {proceso.FechaFin}");
            sb.AppendLine($"Fecha Inicio: {proceso.FechaInicio}");
            sb.AppendLine($"Observaciones: {proceso.Observaciones}");
            sb.AppendLine($"Corporativo: {proceso.Corporativo}");
            sb.AppendLine($"Estado del proceso: {proceso.IdEstadoProcesoNavigation.NombreEstadoProceso}");

            return sb.ToString();
        }
    }
}
