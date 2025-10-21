using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;
using System.Xml.Linq;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class DeclaracionExtensions
    {
        public static DeclaracionDTO ToDeclaracionDTO(this Declaracion declaracion)
        {
            return new DeclaracionDTO
            {


                IdEstadoDeclaracion = declaracion.IdEstadoDeclaracion,
                 IdFormulario = declaracion.IdFormulario,
                IdDeclaracion = declaracion.IdDeclaracion,
                EstadoDeclaracion = declaracion.IdEstadoDeclaracionNavigation.ToEstadoDeclaracionDTO(),
                IdCiudad = declaracion.IdCiudad,
                Formulario = declaracion.IdFormularioNavigation.ToFormularioDTO(),
                FechaDeclaracion = declaracion.FechaDeclaracion,
                ConfirmacionResponsabilidad = declaracion.ConfirmacionResponsabilidad,
                RecibidaEnFisico = declaracion.RecibidaEnFisico,
                Cargo = declaracion.Cargo,
                Nombres = declaracion.Nombres,
                Apellidos = declaracion.Apellidos,
                UnidadOrganizacional = declaracion.UnidadOrganizacional,
                IdFuncionario = declaracion.IdFuncionario,
                bConflictoInteres = declaracion.bConflictoInteres,
                sJustificacion = declaracion.sJustificacion,

                 Funcionario= declaracion.IdFuncionarioNavigation.ToFuncionarioDTO()
                 
            };
        }

        public static ICollection<DeclaracionDTO> ToDeclaracionCollection(this ICollection<Declaracion> declaracion)
        {
            return declaracion.Select(declaracion => declaracion.ToDeclaracionDTO()).ToList();

        }

        public static ICollection<Declaracion> ToDeclaracionDTOCollection(this ICollection<DeclaracionDTO> declaracionDTO)
        {
            return declaracionDTO.Select(declaracion => declaracion.ToDeclaracion()).ToList();

        }
        public static Declaracion ToDeclaracion(this DeclaracionDTO declaracionDTO)
        {
            return new Declaracion
            {

                IdEstadoDeclaracion = declaracionDTO.IdEstadoDeclaracion,
                IdDeclaracion = declaracionDTO.IdDeclaracion,
                IdFormulario = declaracionDTO.IdFormulario,
                IdCiudad = declaracionDTO.IdCiudad.GetValueOrDefault(),
                Cargo = declaracionDTO.Cargo,
                UnidadOrganizacional = declaracionDTO.UnidadOrganizacional,
                FechaDeclaracion = declaracionDTO.FechaDeclaracion,
                ConfirmacionResponsabilidad = declaracionDTO.ConfirmacionResponsabilidad,
                RecibidaEnFisico = declaracionDTO.RecibidaEnFisico,
                Eliminado = false,
                IdFuncionario = declaracionDTO.IdFuncionario.Value,
                 Nombres = declaracionDTO.Nombres,
                Apellidos = declaracionDTO.Apellidos,
                bConflictoInteres = declaracionDTO.bConflictoInteres,
                sJustificacion = declaracionDTO.sJustificacion,
                LugarTrabajo = declaracionDTO.LugarTrabajo,

                Vicepresidencia = declaracionDTO.Vicepresidencia,
                Siglas = declaracionDTO.Siglas

            };
        }

        public static string ToAuditoria(this DeclaracionDTO declaracionDTO)
        {
            if (declaracionDTO == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();
            sb.AppendLine($"ID: {declaracionDTO.IdDeclaracion}");
            sb.AppendLine($"Tipo de declaración: {declaracionDTO.Formulario.Titulo}");
            sb.AppendLine($"Estado Declaracion: {declaracionDTO.EstadoDeclaracion.NombreEstadoDeclaracion}");
            sb.AppendLine($"Nombres: {declaracionDTO.Nombres+ declaracionDTO.Apellidos}");
            sb.AppendLine($"Unidad Organizacional: {declaracionDTO.UnidadOrganizacional}");
            sb.AppendLine($"Fecha de Declaracion: {declaracionDTO.FechaDeclaracion}");
            sb.AppendLine($"Justificación: {declaracionDTO.sJustificacion}");
            sb.AppendLine($"ConflictoInteres: {declaracionDTO.bConflictoInteres}");
            if (declaracionDTO.RecibidaEnFisico==false)
            {
                sb.AppendLine($"Recibida en Fisico: Electronica");
            }
            else
            {
                sb.AppendLine($"Recibida en Fisico: Fisica");
            }
  
            sb.AppendLine($"Cargo: {declaracionDTO.Cargo}");
 

            return sb.ToString();
        }
    }
}
