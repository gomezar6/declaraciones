using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class DeclaracionFuncionarioCargoDTO
{

    public int IdDeclaracion { get; set; }
    public int IdFuncionario { get; set; }
    public string Cargo { get; set; } = null!;
    public string UnidadOrganizacional { get; set; } = null!;
    public DateTime Fecha { get; set; }
   


}