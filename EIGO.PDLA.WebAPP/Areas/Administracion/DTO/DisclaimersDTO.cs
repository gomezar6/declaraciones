using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class DisclaimerDTO
{
    public int IdDisclaimer { get; set; }
    [Required, Display(Name = "Proceso")]
    public int IdProceso { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Título"), MaxLength(100)]
    public string Titulo { get; set; } = null!;
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Cuerpo")]
    public string? Texto { get; set; }

    public Disclaimer ToDisclaimers()
    {
        return new Disclaimer
        {
            IdDisclaimer = IdDisclaimer,
            Titulo = Titulo,
            Texto = Texto,
            IdProceso = IdProceso,
            Eliminado = false // TODO: Validar que esto no reactive los eliminados
        };
    }
}
