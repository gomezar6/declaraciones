using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class SincronizacionDTO
{
    public int IdSincronizacion { get; set; }
    public DateTime Fecha { get; set; }
    [Display(Name = "Estado de sincronización")]
    public string EstatusSincronizacion { get; set; } = null!;
    [Display(Name = "Cantidad de registros")]
    public short CantidadRegistros { get; set; }
    [Display(Name = "Registros insertados")]
    public short RegistrosInsertados { get; set; }
    [Display(Name = "Registros modificados")]
    public short RegistrosModificados { get; set; }
    [Display(Name = "Registros con errores")]
    public short RegistrosConErrores { get; set; }
    [Display(Name = "Tipo de proceso")]
    public string TipoProceso { get; set; } = null!;

    public virtual ICollection<SincronizacionDetalleDTO> SincronizacionDetalles { get; set; } = null!;
}
