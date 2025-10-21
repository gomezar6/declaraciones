namespace EIGO.PDLA.Common.Models;
public class Ciudad
{
    public Ciudad()
    {
        Declaraciones = new HashSet<Declaracion>();
    }

    public int IdPais { get; set; }
    public int IdCiudad { get; set; }
    public string NombreCiudad { get; set; } = null!;
    public bool? Eliminado { get; set; }
    public DateTime Creado { get; set; }
    public string CreadoPor { get; set; } = null!;
    public DateTime? Modificado { get; set; }
    public string? ModificadoPor { get; set; }

    public virtual Pais IdPaisNavigation { get; set; } = null!;
    public virtual ICollection<Declaracion> Declaraciones { get; set; }
}
