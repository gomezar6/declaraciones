namespace EIGO.PDLA.Common.Models;

public partial class FuncionarioNacionalidad : Auditable
{
    public int Id { get; set; }
    public int? IdDeclaracion { get; set; }
    public int IdFuncionario { get; set; }
    public int Nacionalidad { get; set; }
    public virtual Pais PaisNavigation { get; set; } = null!;

}
