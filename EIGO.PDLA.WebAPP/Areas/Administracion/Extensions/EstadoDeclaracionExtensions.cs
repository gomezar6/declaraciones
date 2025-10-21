using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class EstadoDeclaracionExtensions
    {
        public static EstadoDeclaracionDTO ToEstadoDeclaracionDTO(this EstadoDeclaracion estadoDeclaracion)
        {
            return new EstadoDeclaracionDTO
            {
                IdEstado = estadoDeclaracion.IdEstado,
                 NombreEstadoDeclaracion = estadoDeclaracion.NombreEstadoDeclaracion
            };
        }
        public static EstadoDeclaracion ToEstadoDeclaracion(this EstadoDeclaracionDTO EstadoDeclaracionDTO)
        {
            return new EstadoDeclaracion
            {
                IdEstado = EstadoDeclaracionDTO.IdEstado,
                NombreEstadoDeclaracion = EstadoDeclaracionDTO.NombreEstadoDeclaracion
            };
        }
    }
}
