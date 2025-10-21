namespace EIGO.PDLA.Common.Models
{
    public partial class Parentesco : Auditable
    {
        public Parentesco()
        {
            Familiares = new HashSet<Familiar>();
        }

        public int IdParentesco { get; set; }
        public string NombreParentesco { get; set; } = null!;

        public virtual ICollection<Familiar> Familiares { get; set; }
        public virtual ICollection<Participacion> Participaciones { get; set; }
    }
}
