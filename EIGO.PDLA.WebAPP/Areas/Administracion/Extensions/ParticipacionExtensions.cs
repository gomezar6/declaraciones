using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class ParticipacionExtensions
    {
        public static ParticipacionDto ToParticipacionDto(this Participacion participacion)
        {
            if (participacion == null)
                return null;
            return new ParticipacionDto
            {
                IdPais = participacion.IdPais,
                IdParticipacion = participacion.IdParticipacion,
                Cargo = participacion.Cargo,
                NombreEmpresa = participacion.NombreEmpresa,
                PctAccionario = participacion.PctAccionario,
                IdDeclaracion = participacion.IdDeclaracion,
                NombreFamiliar = participacion.NombreFamiliar,
                ApellidoFamiliar = participacion.ApellidoFamiliar,
                IdParentesco = participacion.IdParentesco,
                Eliminado = participacion.Eliminado,
                  bOtro = participacion.bOtro,
                dMesAnioInicio = participacion.dMesAnioInicio,
                tipoCargo = participacion.ntipoCargo
            };
        }

        public static ParticipacionReportePorAccionarioDTO ToParticipacionDtoPorAccionario(this Participacion participacion)
        {
            if (participacion == null)
                return null;
            return new ParticipacionReportePorAccionarioDTO
            {
                NombreEmpresa = participacion.NombreEmpresa,
                Pais = participacion.IdDeclaracionNavigation.IdCiudadNavigation.IdPaisNavigation.NombrePais,
                NombreFamiliar = participacion.NombreFamiliar,
                PctAccionario = participacion.PctAccionario,
                Funcionario = participacion.IdDeclaracionNavigation.Nombres + " " + participacion.IdDeclaracionNavigation.Apellidos,
                Área = participacion.IdDeclaracionNavigation.UnidadOrganizacional,
                Parentesco = participacion.IdParentescoNavigation.NombreParentesco,
                    bOtro = participacion.bOtro,
                dMesAnioInicio = participacion.dMesAnioInicio,
                tipoCargo = participacion.ntipoCargo

            };
        }

        public static ICollection<ParticipacionReportePorAccionarioDTO> ToParticipacionDtoPorAccionarioCollection(this ICollection<Participacion> participacion)
        {
            return participacion.Select(Participacion => Participacion.ToParticipacionDtoPorAccionario()).ToList();
        }
        public static ParticipacionReporteCargoDTO ToParticipacionDtoCargo(this Participacion participacion)
        {
            if (participacion == null)
                return null;
            return new ParticipacionReporteCargoDTO
            {
                NombreEmpresa = participacion.NombreEmpresa,
                Pais = participacion.IdDeclaracionNavigation.IdCiudadNavigation.IdPaisNavigation.NombrePais,
                NombreFamiliar = participacion.NombreFamiliar,
                Cargo = participacion.Cargo,
                Funcionario = participacion.IdDeclaracionNavigation.Nombres+" "+ participacion.IdDeclaracionNavigation.Apellidos,
                Área = participacion.IdDeclaracionNavigation.UnidadOrganizacional,
                Parentesco = participacion.IdParentescoNavigation.NombreParentesco,
                bOtro = participacion.bOtro,
                dMesAnioInicio= participacion.dMesAnioInicio,
                 tipoCargo = participacion.ntipoCargo
            };
        }
        public static ICollection<ParticipacionReporteCargoDTO> ToParticipacionDtoCargoCollection(this ICollection<Participacion> participacion)
        {
            return participacion.Select(Participacion => Participacion.ToParticipacionDtoCargo()).ToList();
        }

        public static ICollection<ParticipacionDto> ToParticipacionDtoCollection(this ICollection<Participacion> participacion)
        {
            return participacion.Select(Participacion => Participacion.ToParticipacionDto()).ToList();
        }

        public static string ToAuditoria(this Participacion participacion)
        {
            if (participacion == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();
            sb.AppendLine($"Cargo: {participacion.Cargo}");
            sb.AppendLine($"Nombre de Empresa: {participacion.NombreEmpresa}");
            sb.AppendLine($"Pct Accionario: {participacion.PctAccionario}");
            sb.AppendLine($"Nombre Familiar: {participacion.NombreFamiliar}");


            return sb.ToString();
        }
    }
}
