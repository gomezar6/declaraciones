namespace EIGO.PDLA.Common.Models
{
    public partial class ParticipacionEntidad : Auditable
    {
        public ParticipacionEntidad()
        {
            //Participaciones = new HashSet<ParticipacionEntidad>();
        }

        public int IdParticipacionEntidad { get; set; }
        public string Participacion { get; set; } = null!;                
        // public virtual ICollection<ParticipacionEntidad> Participaciones { get; set; }
        
    }
}
