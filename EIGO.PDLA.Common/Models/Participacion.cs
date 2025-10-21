namespace EIGO.PDLA.Common.Models
{
    public partial class Participacion : Auditable
    {
        public int IdParticipacion { get; set; }
        public int IdDeclaracion { get; set; }
        public int IdPais { get; set; }
        public int IdParentesco { get; set; }                
        public string? Cargo { get; set; }
        public byte? ntipoCargo { get; set; }
        public string? NombreFamiliar { get; set; }
        public string? ApellidoFamiliar { get; set; }
        public string? NombreEmpresa { get; set; }
        public decimal? PctAccionario { get; set; }  
        public bool? bOtro {  get; set; }
        public DateTime? dMesAnioInicio { get; set; }
        public virtual Declaracion IdDeclaracionNavigation { get; set; } = null!;
        public virtual Pais IdPaisNavigation { get; set; } = null!;
        public virtual Parentesco IdParentescoNavigation { get; set; } = null!;
    }
}
