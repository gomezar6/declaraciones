using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class DisclaimersExtensions
    {
        public static DisclaimerDTO ToDisclaimerDTO(this Disclaimer disclaimer)
        {
            return new DisclaimerDTO
            {
                IdDisclaimer = disclaimer.IdDisclaimer,
                IdProceso = disclaimer.IdProceso,
                Texto = disclaimer.Texto,
                Titulo = disclaimer.Titulo
            };
        }

        public static ICollection<DisclaimerDTO> ToDisclaimerDTOCollection(this ICollection<Disclaimer> disclaimers)
        {
            return disclaimers.Select(disclaimer => disclaimer.ToDisclaimerDTO()).ToList();
        }

        public static Disclaimer ToDisclaimer(this DisclaimerDTO disclaimerDTO)
        {
            return new Disclaimer
            {
                IdProceso = disclaimerDTO.IdProceso,
                IdDisclaimer = disclaimerDTO.IdDisclaimer,
                Texto = disclaimerDTO.Texto,
                Titulo = disclaimerDTO.Titulo
            };
        }
        public static string ToAuditoria(this DisclaimerDTO disclaimerDTO)
        {
            if (disclaimerDTO == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();
            sb.AppendLine($"Titulo: {disclaimerDTO.Titulo}");
            sb.AppendLine($"Texto: {disclaimerDTO.Texto}");
            
   

            return sb.ToString();
        }
    }
}
