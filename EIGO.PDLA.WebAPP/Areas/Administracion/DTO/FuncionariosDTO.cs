using System.ComponentModel.DataAnnotations;
namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO
{
    public class FuncionariosDTO
    {
        public bool IsSelected { get; set; }
        public int IdFuncionario { get; set; }
        public int Cup { get; set; }
        public string Nombres { get; set; } = null!;
        public string? Apellidos { get; set; }
        public string Cargo { get; set; } = null!;
        [Display(Name = "Área")]
        public string UnidadOrganizacional { get; set; } = null!;
        public string? Siglas { get; set; }
        [Display(Name = "Lugar de Trabajo")]
        public string LugarTrabajo { get; set; } = null!;
       
        public string? Email { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string? EstadoAusencia { get; set; }
        public DateTime? FechaInicioAusencia { get; set; }
        public DateTime? FechaFinAusencia { get; set; }
    }
}
