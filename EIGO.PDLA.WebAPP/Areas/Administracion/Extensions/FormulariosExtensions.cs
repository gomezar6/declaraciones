using EIGO.PDLA.Common.Models;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.Text;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Extensions
{
    public static class FormulariosExtensions
    {
        public static FormularioDTO ToFormularioDTO(this Formulario formulario)
        {
            return new FormularioDTO
            {
                Encabezado = formulario.Encabezado,
                //Fecha = formulario.Fecha,
                IdFormulario = formulario.IdFormulario,
                IdProceso = formulario.IdProceso,
                IdTipoDeclaracion = formulario.IdTipoDeclaracion,
                PiePagina = formulario.PiePagina,
                RecibirEnFisico = formulario.RecibirEnFisico,
                Texto1 = formulario.Texto1,
                Texto2 = formulario.Texto2,
                Texto3 = formulario.Texto3,
                Texto4 = formulario.Texto4,
                Texto5 = formulario.Texto5,
                TipoDeclaracion = formulario.IdTipoDeclaracionNavigation.ToTipoDeclaracionDTO(),
                ProcesoDisclaimerFormulario = formulario.ProcesoDisclaimerFormulario.ToProcesoDisclaimerFormularioDTOCollection(),
                Titulo = formulario.Titulo,
                VersionFormulario = formulario.VersionFormulario
            };
        }

        public static ICollection<FormularioDTO> ToFormularioDTOCollection(this ICollection<Formulario> formularios)
        {
            return formularios.Select(formulario => formulario.ToFormularioDTO()).ToList();

        }
        public static Formulario ToFormulario(this FormularioDTO formularioDTO)
        {
            return new Formulario
            {
                Encabezado = formularioDTO.Encabezado,
                //Fecha = formularioDTO.Fecha,
                IdFormulario = formularioDTO.IdFormulario,
                IdProceso = formularioDTO.IdProceso,
                PiePagina = formularioDTO.PiePagina,
                RecibirEnFisico = formularioDTO.RecibirEnFisico,
                IdTipoDeclaracion = formularioDTO.IdTipoDeclaracion,
                Texto1 = formularioDTO.Texto1,
                Texto2 = formularioDTO.Texto2,
                Texto3 = formularioDTO.Texto3,
                Texto4 = formularioDTO.Texto4,
                Texto5 = formularioDTO.Texto5,
                Titulo = formularioDTO.Titulo,
                VersionFormulario = formularioDTO.VersionFormulario,
             
                Eliminado = false
            };
        }

        public static string ToAuditoria(this FormularioDTO formularioDTO)
        {
            if (formularioDTO == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();
           
            sb.AppendLine($"Nombre: {formularioDTO.Titulo}");
            sb.AppendLine($"Pie de pagina: {formularioDTO.PiePagina}");
            sb.AppendLine($"Encabeza: {formularioDTO.Encabezado}");
            sb.AppendLine($"Seccion 1: {formularioDTO.Texto1}");
            sb.AppendLine($"Seccion 2: {formularioDTO.Texto2}");
            sb.AppendLine($"Seccion 3: {formularioDTO.Texto3}");
            sb.AppendLine($"Seccion 4: {formularioDTO.Texto4}");
            sb.AppendLine($"RecibirEnFisico: {formularioDTO.RecibirEnFisico}");


            return sb.ToString();
        }


        public static string ToAuditoria(this Formulario formularioDTO)
        {
            if (formularioDTO == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new();
           
            sb.AppendLine($"Nombre: {formularioDTO.Titulo}");
            sb.AppendLine($"Pie de pagina: {formularioDTO.PiePagina}");
            sb.AppendLine($"Encabeza: {formularioDTO.Encabezado}");
            sb.AppendLine($"Seccion 1: {formularioDTO.Texto1}");
            sb.AppendLine($"Seccion 2: {formularioDTO.Texto2}");
            sb.AppendLine($"Seccion 3: {formularioDTO.Texto3}");
            sb.AppendLine($"Seccion 4: {formularioDTO.Texto4}");
            sb.AppendLine($"RecibirEnFisico: {formularioDTO.RecibirEnFisico}");


            return sb.ToString();
        }
    }
}
