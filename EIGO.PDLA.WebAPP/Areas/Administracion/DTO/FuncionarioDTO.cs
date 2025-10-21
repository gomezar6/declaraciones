using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class FuncionarioDTO
{


    public int IdFuncionario { get; set; }
    public int IdPersona { get; set; }
    public int Cup { get; set; }

    [Display(Name = "Nombres")]
    public string Nombres { get; set; } = null!;
    [Display(Name = "Apellidos")]
    public string? Apellidos { get; set; }
    [Display(Name = "Cargo")]
    public string Cargo { get; set; } = null!;
    [Display(Name = "Área")]
    public string UnidadOrganizacional { get; set; } = null!;
    public string? Siglas { get; set; }
    [Display(Name = "Lugar de Trabajo")]
    public string LugarTrabajo { get; set; } = null!;
    //public int IdPais { get; set; }
    //public string Nacionalidad { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string? EstadoAusencia { get; set; }
    public DateTime? FechaInicioAusencia { get; set; }
    public DateTime? FechaFinAusencia { get; set; }


}