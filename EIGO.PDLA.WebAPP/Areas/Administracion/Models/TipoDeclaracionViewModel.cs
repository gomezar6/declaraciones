using EIGO.PDLA.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Models
{
    public class CrearTipoDeclaracionViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        public TipoDeclaracion ToTipoDeclaracion()
        {
            return new TipoDeclaracion
            {
                NombreDeclaracion = Nombre
            };
        }
    }
}
