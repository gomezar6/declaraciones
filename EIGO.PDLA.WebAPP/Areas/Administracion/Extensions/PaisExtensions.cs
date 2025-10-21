using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class PaisExtensions
    {
        public static PaisDTO ToPaisDTO(this Pais pais)
        {
            if (pais == null)
                return null;
            return new PaisDTO
            {
                IdPais = pais.IdPais,
                NombrePais = pais.NombrePais,
                PresenciaCaf = pais.PresenciaCaf
            };
        }

       
    }
}
