namespace EIGO.PDLA.Common.Models
{
    public partial class DeclaracionFuncionarioCargos : Auditable
    {
        

        public int IdDeclaracion { get; set; }
        public int IdFuncionario { get; set; }
        public string Cargo { get; set; } = null!;
        public string UnidadOrganizacional { get; set; } = null!;

        //public virtual Declaracion IdDeclaracionNavigation { get; set; } = null!;
        public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;
    }
}
