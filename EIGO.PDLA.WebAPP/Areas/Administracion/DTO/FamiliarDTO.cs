using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class FamiliarDTO
{


    public int IdFamiliar { get; set; }
    public int IdFuncionario { get; set; }

    public int IdParentesco { get; set; }
    public string NombreFamiliar { get; set; } = null!;
    public string? ApellidoFamiliar { get; set; }

    public Familiar ToFamiliar()
    {
        return new Familiar
        {
            IdFuncionario = IdFuncionario,
            IdParentesco = IdParentesco,
            ApellidoFamiliar = ApellidoFamiliar,
            NombreFamiliar = NombreFamiliar
        };
    }
}
