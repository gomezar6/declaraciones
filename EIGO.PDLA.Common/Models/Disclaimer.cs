namespace EIGO.PDLA.Common.Models
{
    public partial class Disclaimer : Auditable
    {
        public int IdDisclaimer { get; set; }
        public int IdProceso { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Texto { get; set; }

        public virtual Proceso IdProcesoNavigation { get; set; } = null!;
    }
}
