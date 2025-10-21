namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class FuncionariosProcesoAjaxDto
{
    public int IdProceso { get; set; }
    public List<int> Funcionarios { get; set; } = new List<int>();
}
