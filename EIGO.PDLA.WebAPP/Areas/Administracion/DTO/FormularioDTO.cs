using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class FormularioDTO
{
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Tipo de Declaración")]
    public int IdTipoDeclaracion { get; set; }
    public int IdFormulario { get; set; }
    [Required, Display(Name = "Proceso")]
    public int IdProceso { get; set; }
    //[Required, DataType(DataType.Date)]
    //public DateTime Fecha { get; set; }
    [Display(Name = "Título"), MaxLength(200)]
    public string? Titulo { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Versión del Formulario")]

    public short VersionFormulario { get; set; }

    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Encabezado")]
    public string Encabezado { get; set; } = null!;
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Pie de Página")]
    public string PiePagina { get; set; } = null!;
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Recibir en Físico")]
    public bool RecibirEnFisico { get; set; }
    [Display(Name = "Sección 1")]
    public string? Texto1 { get; set; }
    [Display(Name = "Sección 2")]
    public string? Texto2 { get; set; }
    [Display(Name = "Sección 3")]
    public string? Texto3 { get; set; }
    [Display(Name = "Sección 4")]
    public string? Texto4 { get; set; }
    [Display(Name = "Sección 5")]
    public string? Texto5 { get; set; }


    public bool Preview { get; set; }

    [ Display(Name = "Tipo de Declaración")]
    public virtual TipoDeclaracionDTO? TipoDeclaracion { get; set; } 
    public virtual ICollection<ProcesoDisclaimerFormularioDTO> ProcesoDisclaimerFormulario { get; set; } = null!;

}