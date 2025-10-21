namespace EIGO.PDLA.Common.Models;
public partial class Alerta : Auditable
{
    public int IdAlerta { get; set; }
    public int IdProceso { get; set; }
    public DateTime Fecha { get; set; }
    public string Asunto { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string? SubTitulo { get; set; }
    public string Cuerpo { get; set; } = null!;
    public bool Diligenciado { get; set; }
    public bool Estatus { get; set; }
    public string AvisoConfidencialidad { get; set; } = null!;

    public virtual Proceso IdProcesoNavigation { get; set; } = null!;
}
