namespace EIGO.PDLA.Common.Models
{
    public partial class EstadoDeclaracion : Auditable
    {
        public EstadoDeclaracion()
        {
            Declaraciones = new HashSet<Declaracion>();
        }

        public int IdEstado { get; set; }
        public string NombreEstadoDeclaracion { get; set; } = null!;

        public virtual ICollection<Declaracion> Declaraciones { get; set; }
    }
}
