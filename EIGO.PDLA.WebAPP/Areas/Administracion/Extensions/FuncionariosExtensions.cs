using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
public static class FuncionariosExtensions
{
        public static FuncionarioDTO ToFuncionarioDTO(this Funcionario funcionario)
    {
            return new FuncionarioDTO
            {
                Apellidos = funcionario.Apellidos,
                Cargo = funcionario.Cargo,
                Cup = funcionario.Cup,
                Email = funcionario.Email,
                EstadoAusencia = funcionario.EstadoAusencia,
                FechaFinAusencia = funcionario.FechaFinAusencia,
                FechaIngreso = funcionario.FechaIngreso,
                FechaInicioAusencia = funcionario.FechaInicioAusencia,
                LugarTrabajo = funcionario.LugarTrabajo,
    
                Nombres = funcionario.Nombres,
                Siglas = funcionario.Siglas,
                UnidadOrganizacional = funcionario.UnidadOrganizacional,
                IdFuncionario = funcionario.IdFuncionario,
                IdPersona = funcionario.IdPersona

            };
        }
        public static List<FuncionariosDTO> ToFuncionariosCollectionDTO(this List<Funcionario> funcionarios, List<ProcesosFuncionario> procesosFuncionarios,     bool corporativo)
        {
            return funcionarios.Select(f => new FuncionariosDTO
            {
                Apellidos = f.Apellidos,
                EstadoAusencia = f.EstadoAusencia,
                FechaFinAusencia = f.FechaFinAusencia,
                FechaInicioAusencia = f.FechaInicioAusencia,
                Cargo = f.Cargo,
                Cup = f.Cup,
                Email = f.Email,
                FechaIngreso = f.FechaIngreso,
                IdFuncionario = f.IdFuncionario,
                IsSelected = corporativo || procesosFuncionarios.Any(pf => pf.IdFuncionario == f.IdFuncionario),
                LugarTrabajo = f.LugarTrabajo,
         
                Nombres = f.Nombres,
                Siglas = f.Siglas,
                UnidadOrganizacional = f.UnidadOrganizacional
            }).ToList();
        }
        public static ICollection<FuncionarioDTO> ToFuncionariosDTOCollection(this ICollection<Funcionario> funcionarios)
        {
            return funcionarios.Select(funcionario => funcionario.ToFuncionarioDTO()).ToList();

        }
        public static Funcionario ToFuncionario(this FuncionarioDTO funcionarioDTO)
        {
            return new Funcionario
        {
                Apellidos = funcionarioDTO.Apellidos,
                Cargo = funcionarioDTO.Cargo,
                Cup = funcionarioDTO.Cup,
                Email = funcionarioDTO.Email,
                EstadoAusencia = funcionarioDTO.EstadoAusencia,
                FechaFinAusencia = funcionarioDTO.FechaFinAusencia,
                FechaIngreso = funcionarioDTO.FechaIngreso,
                FechaInicioAusencia = funcionarioDTO.FechaInicioAusencia,
                LugarTrabajo = funcionarioDTO.LugarTrabajo,
      
                Nombres = funcionarioDTO.Nombres,
                Siglas = funcionarioDTO.Siglas,
                UnidadOrganizacional = funcionarioDTO.UnidadOrganizacional,
                IdFuncionario = funcionarioDTO.IdFuncionario,
                IdPersona = funcionarioDTO.IdPersona,
                Eliminado = false
            };
        }

       
    }

}
