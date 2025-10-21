namespace EIGO.PDLA.Common.Models
{
    public class ProcesosFuncionario
    {
        public int IdProceso { get; set; }
        public int IdFuncionario { get; set; }

        public virtual Funcionario IdFuncionarioNavigation { get; set; } = null!;
        public virtual Proceso IdProcesoNavigation { get; set; } = null!;

        public bool Eliminado { get; set; }
        public DateTime Creado { get; set; }
        public string CreadoPor { get; set; }
        public DateTime Modificado { get; set; }
        public string? ModificadoPor { get; set; }
    }
}
