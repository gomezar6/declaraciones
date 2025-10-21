namespace EIGO.PDLA.Common.Models
{
    public partial class Pais : Auditable
    {
        public Pais()
        {
            Ciudades = new HashSet<Ciudad>();
            Participaciones = new HashSet<Participacion>();
            FuncionarioNacionalidadNavigation = new HashSet<FuncionarioNacionalidad>();
        }

        public int IdPais { get; set; }
        public string NombrePais { get; set; } = null!;
        public bool? PresenciaCaf { get; set; }

        public virtual ICollection<Ciudad> Ciudades { get; set; }
        public virtual ICollection<Participacion> Participaciones { get; set; }
        public virtual ICollection<FuncionarioNacionalidad> FuncionarioNacionalidadNavigation { get; set; }
    }
}
