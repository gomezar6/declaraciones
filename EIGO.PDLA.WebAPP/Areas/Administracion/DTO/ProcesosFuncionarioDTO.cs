namespace EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
public class ProcesosFuncionarioDTO
{
    public int IdProceso { get; set; }
    public int IdFuncionario { get; set; }
    public bool IsSelected { get; set; }

    public virtual ProcesoDto? IdProcesoNavigation { get; set; }

    public virtual FuncionarioDTO? IdFuncionarioNavigation { get; set; }
}
