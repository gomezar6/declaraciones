using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class PaisDTO
{
  

    public int IdPais { get; set; }
    public string NombrePais { get; set; } = string.Empty;
    public bool? PresenciaCaf { get; set; }

    public Pais ToPais()
    {
        return new Pais
        {
            IdPais = IdPais,
            NombrePais = NombrePais,
            PresenciaCaf = PresenciaCaf
        };
    }
}
