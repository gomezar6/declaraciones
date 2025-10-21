namespace EIGO.PDLA.Common.Models;

public partial class Funcionario : Auditable
{
    public Funcionario()
    {
        Declaraciones = new HashSet<Declaracion>();
        Familiares = new HashSet<Familiar>();
        ProcesosFuncionarios = new HashSet<ProcesosFuncionario>();
    }

    public int IdFuncionario { get; set; }
    public int IdPersona { get; set; }
    public int Cup { get; set; }
    public string Nombres { get; set; } = null!;
    public string? Apellidos { get; set; }
    public string Cargo { get; set; } = null!;
    public string UnidadOrganizacional { get; set; } = null!;
    public string? Vicepresidencia { get; set; }
    public string? Siglas { get; set; }
    public string LugarTrabajo { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string? EstadoAusencia { get; set; }
    public DateTime? FechaInicioAusencia { get; set; }
    public DateTime? FechaFinAusencia { get; set; }

    public virtual ICollection<Declaracion>? Declaraciones { get; set; }
    public virtual ICollection<Familiar>? Familiares { get; set; }
    public virtual ICollection<ProcesosFuncionario>? ProcesosFuncionarios { get; set; }
}
