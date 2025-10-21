namespace EIGO.PDLA.Common.Models
{
    public class SincronizacionDetalle
    {
        public int IdSincronizacionDetalle { get; set; }
        public int IdSincronizacion { get; set; }
        public string Mensajes { get; set; } = null!;

        public virtual Sincronizacion IdSincronizacionNavigation { get; set; } = null!;
    }
}
