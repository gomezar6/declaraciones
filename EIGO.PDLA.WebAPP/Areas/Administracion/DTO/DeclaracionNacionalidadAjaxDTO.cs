

namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class DeclaracionNacionalidadAjaxDTO
{
//    public int IdProceso { get; set; }



 
//public string idFormulario { get; set; }
//public string IdDeclaracion { get; set; }

//public string IdEstadoDeclaracion { get; set; }
//public string RecibidaEnFisico { get; set; }
//public string ConfirmacionResponsabilidad { get; set; }
//public string Cargo { get; set; }
//public string IdPais { get; set; }
//public string IdCiudad { get; set; }
//public string FechaDeclaracion { get; set; }
    public DeclaracionDTO declaracion { get; set; } = new DeclaracionDTO();

    public List<int> Nacionalidades { get; set; } = new List<int>();
    public List<int> NacionalidadesDelete { get; set; } = new List<int>();

    public ParticipacionDto ParticipacionDTO { get; set; } = new ParticipacionDto();
}
