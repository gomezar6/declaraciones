using EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using NUnit.Framework;
using System;

namespace EIGO.PDLA.Tests.WebApp;

public class AttributesTests
{
    [Test]
    public void StartDateEndDateValidationAttributeTest()
    {
        StartDateEndDateValidationAttribute startDateEndDateValidationAttribute = new();
        Assert.IsFalse(startDateEndDateValidationAttribute.IsValid(null));
    }

    [Test]
    public void ProcessDateRangeValidationAttributeTest()
    {
        DateTime startDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        DateTime endDate = startDate.AddDays(30);
        // Validar inicio hoy
        ProcesoDto process = new()
        {
            FechaInicio = startDate,
            FechaFin = endDate

        };
        ProcessDateRangeValidationAttribute processDateRangeValidationAttribute = new();
        Assert.IsTrue(processDateRangeValidationAttribute.IsValid(process));

        process.FechaInicio = startDate.AddDays(-1);

        Assert.IsFalse(processDateRangeValidationAttribute.IsValid(process));

        process.FechaInicio = process.FechaFin == DateTime.MinValue ? DateTime.MaxValue : process.FechaFin.Value;
        Assert.IsFalse(processDateRangeValidationAttribute.IsValid(process));
    }
}
