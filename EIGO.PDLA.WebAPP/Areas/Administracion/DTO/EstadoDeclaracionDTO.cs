using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class EstadoDeclaracionDTO
{
    public int IdEstado { get; set; }
    [Display(Name = "Estado de la Declaración")]
    public string NombreEstadoDeclaracion { get; set; } = null!;


   
}
