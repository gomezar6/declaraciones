namespace EIGO.PDLA.Common.Models
{
    public partial class Auditoria
    {
        public DateTime Fecha { get; set; }
        public string IdUsuario { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public string Ip { get; set; } = null!;
        public int? IdProceso { get; set; }
        public byte? TipoEvento { get; set; }
        public string Evento { get; set; } = null!;
        public string Resultado { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public virtual Proceso? IdProcesoNavigation { get; set; }
    }
}
