using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class AuditoriaExtensions
    {
        public static AuditoriaDTO ToAuditoriaDto(this Auditoria auditoria)
        {
            if (auditoria == null)
                return null;
            return new AuditoriaDTO
            {
                IdProceso = auditoria.IdProceso,
                Descripcion = auditoria.Descripcion,
                Evento = auditoria.Evento,
                Fecha = auditoria.Fecha,
                IdUsuario = auditoria.IdUsuario,
                Resultado = auditoria.Resultado,
                TipoEvento = auditoria.TipoEvento,
                Ip = auditoria.Ip,
                Usuario = auditoria.Usuario

            };
        }

        public static ICollection<AuditoriaDTO> ToParticipacionDtoCollection(this ICollection<Auditoria> auditoria)
        {
            return auditoria.Select(Participacion => Participacion.ToAuditoriaDto()).ToList();
        }


    }
}
