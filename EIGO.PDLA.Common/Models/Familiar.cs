namespace EIGO.PDLA.Common.Models
{
    public partial class Familiar : Auditable
    {
        public int IdFamiliar { get; set; }
        public int IdFuncionario { get; set; }
        public int IdParentesco { get; set; }
        public string NombreFamiliar { get; set; } = null!;
        public string? ApellidoFamiliar { get; set; }
        public int? IdPersona { get; set; }

        public virtual Funcionario? IdFuncionarioNavigation { get; set; }
        public virtual Parentesco IdParentescoNavigation { get; set; } = null!;
    }
}
