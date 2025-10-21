namespace EIGO.PDLA.Common.Models
{
    public partial class EstadoProceso : Auditable
    {
        public EstadoProceso()
        {
            Procesos = new HashSet<Proceso>();
        }

        public int IdEstadoProceso { get; set; }
        public string NombreEstadoProceso { get; set; } = null!;

        public virtual ICollection<Proceso> Procesos { get; set; }
    }
}
