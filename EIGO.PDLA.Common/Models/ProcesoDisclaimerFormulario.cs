namespace EIGO.PDLA.Common.Models
{
    public partial class ProcesoDisclaimerFormulario : Auditable
    {
        public int IdFormulario { get; set; }
        public int IdProceso { get; set; }
        public int IdDisclaimer { get; set; }
        public int Id { get; set; }
        

        public virtual Disclaimer IdDisclaimerNavigation { get; set; } = null!;
        public virtual Formulario IdFormularioNavigation { get; set; } = null!;
        public virtual Proceso IdProcesoNavigation { get; set; } = null!;
    }
}
