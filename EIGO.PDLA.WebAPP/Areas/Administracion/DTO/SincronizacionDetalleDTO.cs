namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO
{
    public class SincronizacionDetalleDTO
    {
        public int IdSincronizacionDetalle { get; set; }
        public int IdSincronizacion { get; set; }
        public string Mensajes { get; set; } = null!;
    }
}
