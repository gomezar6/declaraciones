using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using System.ComponentModel.DataAnnotations;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;

public class AlertasDiligenciadoDuplicadoValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult("La fecha no puede ser nula");
        if (validationContext == null && value == null)
        {
            return new ValidationResult("El objeto no puede ser nulo");
        }

        AlertaDto alerta = value is AlertaDto ? value as AlertaDto : validationContext.ObjectInstance as AlertaDto;


        if (alerta == null)
            return new ValidationResult("El objeto no puede ser nulo");

        IEntityRepository<Proceso> _procesosRepository = (IEntityRepository<Proceso>)validationContext.GetService(typeof(IEntityRepository<Proceso>));

        var procesoDb = _procesosRepository.GetByIdAsync(alerta.IdProceso).Result;

        if (procesoDb != null && alerta.Diligenciado && procesoDb.Alerta.Any(a => a.Diligenciado && a.IdAlerta != alerta.IdAlerta))
        {
            return new ValidationResult("Ya existe una alerta de proceso diligenciado asociada al proceso");
        }

        return ValidationResult.Success;
    }
}
