#nullable disable
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

public class AuditoriaDTO
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
}