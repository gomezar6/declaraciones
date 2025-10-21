using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class ProcesoDisclaimerFormularioDTO
{

    public int IdProceso { get; set; }
    public int IdFormulario { get; set; }
    public int idIdDisclaimer { get; set; }

    public int Id { get; set; }

    
    public virtual DisclaimerDTO Disclaimer { get; set; } = null!;
 
}