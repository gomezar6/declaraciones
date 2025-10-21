using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class ProcesosFuncionarioExtensions
    {
        public static ProcesosFuncionarioDTO ToProcesoFormularioDTO(this ProcesosFuncionario procesosFuncionario)
        {
            return new ProcesosFuncionarioDTO
            {
                 IdFuncionario = procesosFuncionario.IdFuncionario,
                 IdProceso=procesosFuncionario.IdProceso,
   
                   IdFuncionarioNavigation = procesosFuncionario.IdFuncionarioNavigation.ToFuncionarioDTO()
            };
        }

       


        public static ICollection<ProcesosFuncionarioDTO> ToProcesoFormularioDTOCollection(this ICollection<ProcesosFuncionario> procesosFuncionario)
        {
            return procesosFuncionario.Select(_PF => _PF.ToProcesoFormularioDTO()).ToList();

        }
    }
}
