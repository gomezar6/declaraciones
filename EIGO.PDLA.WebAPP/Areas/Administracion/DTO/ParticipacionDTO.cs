namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class ParticipacionDto
{
    public int IdParticipacion { get; set; }
    public int IdDeclaracion { get; set; }
    public int IdPais { get; set; }
    public int IdParentesco { get; set; }
    public string? Cargo { get; set; }
    public byte? tipoCargo { get; set; }
    public string? NombreFamiliar { get; set; }
    public string? ApellidoFamiliar { get; set; }
    public string? NombreEmpresa { get; set; }
    public decimal? PctAccionario { get; set; }
    public bool? Eliminado { get; set; }

    public bool? bOtro { get; set; }
    public DateTime? dMesAnioInicio { get; set; }

}
