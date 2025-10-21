namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;

public class ParticipacionReporteCargoDTO
{
    public string? NombreEmpresa { get; set; }
    public string? Pais { get; set; }

    public string? NombreFamiliar { get; set; }
    public string? Cargo { get; set; }
    public byte? tipoCargo { get; set; }
    public string? Parentesco { get; set; }
    public string? Funcionario { get; set; }
    public string? Área { get; set; }
    public bool? bOtro { get; set; }
    public DateTime? dMesAnioInicio { get; set; }

}
