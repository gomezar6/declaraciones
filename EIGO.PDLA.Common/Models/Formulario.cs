namespace EIGO.PDLA.Common.Models
{
    public partial class Formulario : Auditable
    {
        public Formulario()
        {
            Declaraciones = new HashSet<Declaracion>();
            ProcesoDisclaimerFormulario = new HashSet<ProcesoDisclaimerFormulario>();



        }

        public int IdFormulario { get; set; }
        public int IdProceso { get; set; }
        public int IdTipoDeclaracion { get; set; }
        //public DateTime Fecha { get; set; }
        public short VersionFormulario { get; set; }
        public string Encabezado { get; set; } = null!;
        public string? Titulo { get; set; } 
        public string PiePagina { get; set; } = null!;
        public bool RecibirEnFisico { get; set; }
        public string? Texto1 { get; set; }
        public string? Texto2 { get; set; }
        public string? Texto3 { get; set; }
        public string? Texto4 { get; set; }
        public string? Texto5 { get; set; }
        public virtual Proceso IdProcesoNavigation { get; set; } = null!;
        public virtual TipoDeclaracion? IdTipoDeclaracionNavigation { get; set; } 
        //public virtual Disclaimer IdDisclaimerNavigation { get; set; } = null!;

        public virtual ICollection<Declaracion> Declaraciones { get; set; }
        public virtual ICollection<ProcesoDisclaimerFormulario> ProcesoDisclaimerFormulario { get; set; } 

    }
}
