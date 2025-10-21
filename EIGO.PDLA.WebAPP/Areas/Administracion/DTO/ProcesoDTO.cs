using EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class ProcesoDto
{
    public int IdProceso { get; set; }
    [Required, Display(Name = "Estado del Proceso")]
    public int IdEstadoProceso { get; set; }
    [Required]
    public bool Eliminado { get; set; }
    [Required(ErrorMessage = "El campo es requerido"), Display(Name = "Nombre del Proceso"), MaxLength(200)]
    public string NombreProceso { get; set; } = null!;
    [Required(ErrorMessage = "El campo es requerido"), DataType(DataType.Date), Display(Name = "Fecha de Inicio"), ProcessDateRangeValidation]
    public DateTime FechaInicio { get; set; }
    [DataType(DataType.Date), Display(Name = "Fecha de Fin")]
    public DateTime? FechaFin { get; set; }
    public string? Observaciones { get; set; }
    public bool Corporativo { get; set; } = false;
    public string Responsable { get; set; } = string.Empty;

    public virtual EstadoProcesoDTO? EstadoProceso { get; set; }
    public virtual ICollection<AlertaDto>? Alertas { get; set; }
    public virtual ICollection<DisclaimerDTO>? Disclaimers { get; set; }
    public virtual ICollection<FormularioDTO>? Formularios { get; set; }
    public virtual ICollection<FuncionariosDTO>? Funcionarios { get; set; }
}