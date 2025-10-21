using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Areas.Administracion.Attributes;
using EIGO.PDLA.WebAPP.Areas.Administracion.DTO;
using EIGO.PDLA.WebAPP.Areas.Administracion.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace EIGO.PDLA.WebAPP.Areas.Administracion.Controllers;

[Route("api/[controller]")]
[ApiController, AllowAnonymous, BasicAuthentication]
public class ParticipacionesController : ControllerBase
{
    private readonly IEntityRepository<Participacion> _participacionesRepository;
    public ParticipacionesController(IEntityRepository<Participacion> participacionesRepository)
    {
        _participacionesRepository = participacionesRepository;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<List<ParticipacionDto>> GetAsync()
    {
        List<Participacion> participaciones = await _participacionesRepository.GetAllActiveAsync();

        if (participaciones.Count == 0)
        {
            Participacion dummyParticipacion = new Participacion();

            dummyParticipacion.ApellidoFamiliar = "DUMMY";
            dummyParticipacion.NombreEmpresa = "DUMMY";
            dummyParticipacion.NombreFamiliar = "DUMMY";
            dummyParticipacion.IdPais = 0;
            dummyParticipacion.IdParentesco = 0;
            dummyParticipacion.PctAccionario = 0;
            dummyParticipacion.Cargo = "DUMMY";
            dummyParticipacion.IdDeclaracion = 0;
            dummyParticipacion.IdParticipacion = 0;
            dummyParticipacion.ntipoCargo = 1;
            dummyParticipacion.bOtro = true;
            dummyParticipacion.dMesAnioInicio = new DateTime(2024, 11, 20);
            dummyParticipacion.Eliminado = false;
            participaciones.Add(dummyParticipacion);

        }
        return (List<ParticipacionDto>)participaciones.ToParticipacionDtoCollection();
    }
}
