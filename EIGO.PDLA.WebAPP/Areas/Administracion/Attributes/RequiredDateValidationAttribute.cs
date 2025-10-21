using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Attributes
{
    public class RequiredDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("");
            if (validationContext.ObjectInstance is not AlertaDto alerta)
                return new ValidationResult("El objecto no es una alerta");
            if (alerta.Diligenciado)
            {
                alerta.Fecha = DateTime.MinValue;
            }
            return ValidationResult.Success;
        }
    }
}
