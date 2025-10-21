using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class TipoDeclaracionDTO
{
    [Display(Name = "Id")]
    public int IdTipo { get; set; }
    [Display(Name = "Tipo de Declaración")]
    public string NombreDeclaracion { get; set; } = string.Empty;
    [Display(Name = "Numero de Secciones")]
    public Int16? NumeroSecciones { get; set; }
    
    public TipoDeclaracion ToTipoDeclaracion()
    {
        return new TipoDeclaracion
        { 
             IdTipo = IdTipo,
            NombreDeclaracion = NombreDeclaracion,
            NumeroSecciones= NumeroSecciones
        };
    }
}
