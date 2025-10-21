using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
public class StartDateEndDateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext == null)
            return new ValidationResult("El objeto no puede ser nulo");
        if (validationContext.ObjectInstance is not AlertaDto alerta)
            return new ValidationResult("El objecto no es una alerta");

        if (alerta.Diligenciado)
        {
            return ValidationResult.Success;
        }

        if (value == null)
            return new ValidationResult("La fecha es requerida");
        if (alerta.Fecha < alerta.FechaInicioProceso)
        {
            return new ValidationResult("La fecha es menor a la fecha de inicio del proceso");
        }
        if (alerta.Fecha > alerta.FechaFinProceso)
        {
            return new ValidationResult("La fecha es mayor a la fecha de fin del proceso");
        }
        return ValidationResult.Success;
    }
}
