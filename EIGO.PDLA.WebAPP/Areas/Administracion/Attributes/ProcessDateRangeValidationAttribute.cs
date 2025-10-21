using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
public class ProcessDateRangeValidationAttribute : ValidationAttribute
{
    private static bool AreEqualsOnlyDate(DateTime a, DateTime b)
    {
        DateOnly dateA = DateOnly.FromDateTime(a);
        DateOnly dateB = DateOnly.FromDateTime(b);
        return dateA == dateB;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("La fecha no puede ser nula");
        if (validationContext == null && value == null)
        {
            return new ValidationResult("El objeto no puede ser nulo");
        }

        ProcesoDto proceso = value is ProcesoDto ? value as ProcesoDto : validationContext.ObjectInstance as ProcesoDto;


        if (proceso == null)
            return new ValidationResult("El objeto no puede ser nulo");
        DateTime now = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);


        if (proceso.FechaFin == DateTime.MinValue || proceso.FechaFin == null)
        {

            return ValidationResult.Success;
        }
        if (proceso.FechaInicio < now)
        {
            return new ValidationResult("La fecha de inicio no puede ser menor que la fecha actual");
        }

        if (proceso.FechaInicio > proceso.FechaFin)
        {
            return new ValidationResult("La fecha de inicio no puede ser menor que la fecha de fin");
        }

        if (AreEqualsOnlyDate(proceso.FechaInicio, proceso.FechaFin.Value))
        {
            return new ValidationResult("La fecha de fin debe ser al menos un día luego de la fecha de inicio");
        }

        return ValidationResult.Success;
    }
}
