#nullable disable
using EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

public class AlertaDto
{
    [Required, Display(Name = "Proceso")]
    public int IdProceso { get; set; }
    [Required]
    public int IdAlerta { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), DataType(DataType.Date), StartDateEndDateValidation, RequiredDateValidation]
    public DateTime Fecha { get; set; }
    [Required(ErrorMessage = "El campo es requerido")]
    public string Asunto { get; set; }
    [Required(ErrorMessage = "El campo es requerido")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Subtítulo")]
    public string SubTitulo { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), DataType(DataType.MultilineText)]
    public string Cuerpo { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), DataType(DataType.MultilineText), Display(Name = "Aviso de confidencialidad")]
    public string AvisoConfidencialidad { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Activar alerta")]
    public bool Estatus { get; set; }
    [AlertasDiligenciadoDuplicadoValidation]
    public bool Diligenciado { get; set; }

    public DateTime FechaInicioProceso { get; set; }
    public DateTime FechaFinProceso { get; set; }
}