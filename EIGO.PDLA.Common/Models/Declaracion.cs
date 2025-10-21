namespace EIGO.PDLA.Common.Models;

public partial class Declaracion : Auditable
{
    public Declaracion()
    {
        Participaciones = new HashSet<Participacion>();
    }

    public int IdDeclaracion { get; set; }
    public int IdEstadoDeclaracion { get; set; } 
    public int IdFormulario { get; set; } 
    public int IdFuncionario { get; set; } 
    public int? IdCiudad { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Cargo { get; set; }
    public string? UnidadOrganizacional { get; set; }
    public string? Vicepresidencia { get; set; }
    public string? Siglas { get; set; }
    public string? LugarTrabajo { get; set; }
    public DateTime? FechaDeclaracion { get; set; }
    public bool? ConfirmacionResponsabilidad { get; set; }
    public bool? RecibidaEnFisico { get; set; }
    public string? Observaciones { get; set; }
    public bool? bConflictoInteres { get; set; }
    public string? sJustificacion { get; set; }

    public virtual EstadoDeclaracion IdEstadoDeclaracionNavigation { get; set; } = null!;
    public virtual Formulario IdFormularioNavigation { get; set; } = null!;
    public virtual ICollection<Participacion> Participaciones { get; set; }
    public virtual Ciudad? IdCiudadNavigation { get; set; }
    public virtual Funcionario? IdFuncionarioNavigation { get; set; }
}
