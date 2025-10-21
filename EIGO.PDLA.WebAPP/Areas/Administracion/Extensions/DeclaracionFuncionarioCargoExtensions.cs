using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class DeclaracionFuncionarioCargoExtensions
    {
        public static DeclaracionFuncionarioCargoDTO ToDeclaracionFuncionarioCargoDTO(this DeclaracionFuncionarioCargos DeclaracionFuncionarioCargo)
        {
            return new DeclaracionFuncionarioCargoDTO
            {
                IdDeclaracion = DeclaracionFuncionarioCargo.IdDeclaracion,
                IdFuncionario=DeclaracionFuncionarioCargo.IdFuncionario,
                Cargo= DeclaracionFuncionarioCargo.Cargo,
               
           
                UnidadOrganizacional= DeclaracionFuncionarioCargo.UnidadOrganizacional

                //Disclaimer = procesoDisclaimerFormulario.IdDisclaimerNavigation.ToDisclaimerDTO()

            };
        }

        public static ICollection<DeclaracionFuncionarioCargoDTO> ToDeclaracionFuncionarioCargoDTOCollection(this ICollection<DeclaracionFuncionarioCargos> DeclaracionFuncionarioCargo)
        {
            return DeclaracionFuncionarioCargo.Select(_DeclaracionFuncionarioCargo => _DeclaracionFuncionarioCargo.ToDeclaracionFuncionarioCargoDTO()).ToList();

        }


        public static ICollection<DeclaracionFuncionarioCargos> ToDeclaracionFuncionarioCargoDTOCollection(this ICollection<DeclaracionFuncionarioCargoDTO> DeclaracionFuncionarioCargoDTO)
        {
            return DeclaracionFuncionarioCargoDTO.Select(_DeclaracionFuncionarioCargo => _DeclaracionFuncionarioCargo.ToDeclaracionFuncionarioCargo()).ToList();

        }


        public static DeclaracionFuncionarioCargos ToDeclaracionFuncionarioCargo(this DeclaracionFuncionarioCargoDTO DeclaracionFuncionarioCargo)
        {
            return new DeclaracionFuncionarioCargos
            {
                IdDeclaracion = DeclaracionFuncionarioCargo.IdDeclaracion,
                IdFuncionario = DeclaracionFuncionarioCargo.IdFuncionario,
                Cargo = DeclaracionFuncionarioCargo.Cargo,
          
                UnidadOrganizacional = DeclaracionFuncionarioCargo.UnidadOrganizacional
            };
        }
    }
}
