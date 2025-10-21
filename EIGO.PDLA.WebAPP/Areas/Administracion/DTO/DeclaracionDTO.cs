using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class DeclaracionDTO
{

    public int IdDeclaracion { get; set; }
    [Required, Display(Name = "IdEstadoDeclaracion")]
    public int IdEstadoDeclaracion { get; set; }
    public int IdFormulario { get; set; }
    [Display(Name = "Ciudad")]
    public int? IdCiudad { get; set; }
    public int? IdFuncionario { get; set; }

    [Display(Name = "Cargo")]
    public string? Cargo { get; set; }
    [Display(Name = "Unidad Organizacional")]
    public string? UnidadOrganizacional { get; set; }

    [Display(Name = "Fecha de Declaración")]
    public DateTime? FechaDeclaracion { get; set; }
    [Display(Name = "Confirmación de Responsabilidad")]
    public bool? ConfirmacionResponsabilidad { get; set; }

    [Display(Name = "Recibida en Físico")]
    public bool? RecibidaEnFisico { get; set; }


    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }

    public string? Vicepresidencia { get; set; }
    public string? Siglas { get; set; }
    public string? LugarTrabajo { get; set; }

    public bool? bConflictoInteres { get; set; }
    public string? sJustificacion { get; set; }


    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    public int? CountDisclaimer { get; set; }
    public virtual EstadoDeclaracionDTO? EstadoDeclaracion { get; set; }
    public virtual FormularioDTO? Formulario { get; set; }
    public virtual ICollection<ProcesosFuncionario>? procesosFuncionario { get; set; }
    public virtual List<Participacion>? Participacion { get; set; }
    public virtual List<Participacion>? Responsabilidad { get; set; }
    public virtual List<ProcesoDto>? proceso { get; set; }
    //public virtual ICollection<DeclaracionFuncionarioCargoDTO> DeclaracionFuncionarioCargo { get; set; } = null!;
    public virtual FuncionarioDTO? Funcionario { get; set; }

    public List<FuncionarioNacionalidad>? Nacionalidades { get; set; }


  


}