using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class TipoDeclaracionExtensions
    {
        public static TipoDeclaracionDTO ToTipoDeclaracionDTO(this TipoDeclaracion tipoDeclaracion)
        {
            if (tipoDeclaracion == null)
                return null;
            return new TipoDeclaracionDTO
            { IdTipo = tipoDeclaracion.IdTipo,
            NombreDeclaracion = tipoDeclaracion.NombreDeclaracion,
             NumeroSecciones = tipoDeclaracion.NumeroSecciones
            };
        }

        public static ICollection<TipoDeclaracionDTO> TotipoDeclaracionDTOCollection(this ICollection<TipoDeclaracion> tipoDeclaracion)
        {
            return tipoDeclaracion.Select(_tipoDeclaracion => _tipoDeclaracion.ToTipoDeclaracionDTO()).ToList();

        }


        public static ICollection<TipoDeclaracion> TotipoDeclaracionCollection(this ICollection<TipoDeclaracionDTO> tipoDeclaracionDTO)
        {
            return tipoDeclaracionDTO.Select(_tipoDeclaracionDTO => _tipoDeclaracionDTO.TotipoDeclaracion()).ToList();

        }


        public static TipoDeclaracion TotipoDeclaracion(this TipoDeclaracionDTO tipoDeclaracionDTO)
        {
            return new TipoDeclaracion
            {
                IdTipo = tipoDeclaracionDTO.IdTipo,
                NombreDeclaracion = tipoDeclaracionDTO.NombreDeclaracion,
                NumeroSecciones = tipoDeclaracionDTO.NumeroSecciones
            };
        }

    }
}
