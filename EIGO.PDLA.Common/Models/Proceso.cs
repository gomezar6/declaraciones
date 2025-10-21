namespace EIGO.PDLA.Common.Models
{
    public partial class Proceso : Auditable
    {
        public Proceso()
        {
            Alerta = new HashSet<Alerta>();
            Disclaimers = new HashSet<Disclaimer>();
            Formularios = new HashSet<Formulario>();
            ProcesosFuncionarios = new HashSet<ProcesosFuncionario>();
        }

        public int IdProceso { get; set; }
        public int IdEstadoProceso { get; set; }
        public string NombreProceso { get; set; } = null!;
        public bool? Corporativo { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Observaciones { get; set; }

        public virtual EstadoProceso IdEstadoProcesoNavigation { get; set; } = null!;
        public virtual ICollection<Alerta> Alerta { get; set; }
        public virtual ICollection<Disclaimer> Disclaimers { get; set; }
        public virtual ICollection<Formulario> Formularios { get; set; }
        public virtual ICollection<ProcesosFuncionario> ProcesosFuncionarios { get; set; }
    }
}
