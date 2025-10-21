namespace EIGO.PDLA.Common.Models
{
    public partial class TipoParticipacion : Auditable
    {
        public int IdParticipacion { get; set; }
        public string NombreParticipacion { get; set; } = null!;
    }
}
