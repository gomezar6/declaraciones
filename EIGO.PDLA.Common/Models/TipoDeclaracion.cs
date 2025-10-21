namespace EIGO.PDLA.Common.Models
{
    public partial class TipoDeclaracion : Auditable
    {
        public TipoDeclaracion()
        {
            Formularios = new HashSet<Formulario>();
            //Procesos = new HashSet<Proceso>();
        }

        public int IdTipo { get; set; }
        public string NombreDeclaracion { get; set; } = null!;
        public Int16? NumeroSecciones { get; set; }

        public virtual ICollection<Formulario> Formularios { get; set; }
        //public virtual ICollection<Proceso> Procesos { get; set; }
    }
}
