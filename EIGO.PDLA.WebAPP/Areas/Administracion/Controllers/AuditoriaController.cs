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

public class AuditoriaController : Controller
{
    private readonly IEntityRepository<Auditoria> _auditoriaRepository;

    public AuditoriaController(IEntityRepository<Auditoria> auditoriaRepository)
    {
        _auditoriaRepository = auditoriaRepository;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<List<AuditoriaDTO>> GetAsync()
    {
        List<Auditoria> auditoria = await _auditoriaRepository.GetAllActiveAsync();
        if (auditoria.Count == 0)
        {
            Auditoria dummyAudit = new Auditoria();

            dummyAudit.IdProceso =null ;
            dummyAudit.TipoEvento = null;
            dummyAudit.Evento = "DUMMY";
            dummyAudit.Resultado = "DUMMY";
            dummyAudit.Descripcion = "DUMMY";
            dummyAudit.Fecha = DateTime.Now;
            dummyAudit.Ip = "DUMMY";
            dummyAudit.IdUsuario = "DUMMY";
            dummyAudit.Usuario = "DUMMY";

            auditoria.Add(dummyAudit);

        }
        return (List<AuditoriaDTO>)auditoria.ToParticipacionDtoCollection();
    }


    //[HttpGet]
    //[EnableQuery]
    //public async Task<List<AuditoriaDTO>> GetbyProcesoAsync(int idProceso)
    //{
    //    List<Auditoria> auditoria = await ((AuditoriaRepository)_auditoriaRepository).GetByIdProcesoAsync(idProceso);
    //    return (List<AuditoriaDTO>)auditoria.ToParticipacionDtoCollection();
    //}
}

