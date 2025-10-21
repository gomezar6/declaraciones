namespace EIGO.PDLA.Common.Models
{
    public partial class Sincronizacion : Auditable
    {
        public int IdSincronizacion { get; set; }
        public DateTime Fecha { get; set; }
        public string EstatusSincronizacion { get; set; } = null!;
        public short CantidadRegistros { get; set; }
        public short RegistrosInsertados { get; set; }
        public short RegistrosModificados { get; set; }
        public short RegistrosConErrores { get; set; }
        public string TipoProceso { get; set; } = null!;

        public virtual ICollection<SincronizacionDetalle> SincronizacionDetalles { get; set; } = null!;
    }
}
