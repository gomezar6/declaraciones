namespace EIGO.PDLA.Common.Models
{
    public class Auditable
    {
        public bool Eliminado { get; set; }
        public DateTime Creado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime Modificado { get; set; }
        public string? ModificadoPor { get; set; }
    }
}
