using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class EstadoProcesoDTO
{
    [Display(Name = "Id")]
    public int IdEstadoProceso { get; set; }
    [Display(Name = "Estado del Proceso")]
    public string NombreEstadoProceso { get; set; } = string.Empty;

    public EstadoProceso ToEstadoProceso()
    {
        return new EstadoProceso
        {
            IdEstadoProceso = IdEstadoProceso,
            NombreEstadoProceso = NombreEstadoProceso
        };
    }
}
