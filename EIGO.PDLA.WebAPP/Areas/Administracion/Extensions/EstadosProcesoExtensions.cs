using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class EstadosProcesoExtensions
    {
        public static EstadoProcesoDTO ToEstadoProcesoDTO(this EstadoProceso estadoProceso)
        {
            return new EstadoProcesoDTO
            {
                IdEstadoProceso = estadoProceso.IdEstadoProceso,
                NombreEstadoProceso = estadoProceso.NombreEstadoProceso
            };
        }
    }
}
