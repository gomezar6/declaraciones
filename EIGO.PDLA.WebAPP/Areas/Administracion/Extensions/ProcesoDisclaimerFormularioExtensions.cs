using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class ProcesoDisclaimerFormularioExtensions
    {
        public static ProcesoDisclaimerFormularioDTO ToProcesoDisclaimerFormularioDTO(this ProcesoDisclaimerFormulario procesoDisclaimerFormulario)
        {
            return new ProcesoDisclaimerFormularioDTO
            {
                IdFormulario = procesoDisclaimerFormulario.IdFormulario,
                IdProceso = procesoDisclaimerFormulario.IdProceso,
                 
                idIdDisclaimer = procesoDisclaimerFormulario.IdDisclaimer,
                Disclaimer = procesoDisclaimerFormulario.IdDisclaimerNavigation.ToDisclaimerDTO()
                 
            };
        }

        public static ICollection<ProcesoDisclaimerFormularioDTO> ToProcesoDisclaimerFormularioDTOCollection(this ICollection<ProcesoDisclaimerFormulario> procesoDisclaimerFormularios)
        {
            return procesoDisclaimerFormularios.Select(_procesoDisclaimerFormulario => _procesoDisclaimerFormulario.ToProcesoDisclaimerFormularioDTO()).ToList();

        }
        public static ProcesoDisclaimerFormulario ToFormulario(this ProcesoDisclaimerFormularioDTO procesoDisclaimerFormulario)
        {
            return new ProcesoDisclaimerFormulario
            {
                IdDisclaimer = procesoDisclaimerFormulario.idIdDisclaimer,
                IdFormulario = procesoDisclaimerFormulario.IdFormulario,
                IdProceso = procesoDisclaimerFormulario.IdProceso
            };
        }
    }
}
