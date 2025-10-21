#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class ParticipacionRepository : IEntityRepository<Participacion>
{
    private readonly DeclaracionesContext _context;

    public ParticipacionRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<Participacion> AddAsync(Participacion entity)
    {
        var result = await _context.Participaciones.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdParticipacion);
    }
    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public async Task<Participacion> DeleteAsync(Participacion entity)
    {
        _context.Participaciones.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Participaciones.AnyAsync(ep => ep.IdParticipacion == id);
    }

    public async Task<bool> Exist(int IdParticipacion, int IdDeclaracion)
    {
        return await _context.Participaciones.AnyAsync(ep => ep.IdParticipacion == IdParticipacion && ep.IdDeclaracion == IdDeclaracion);
    }
    public async Task<List<Participacion>> GetAllActiveAsync()
    {
        return await _context.Participaciones.Where(ep => ep.Eliminado == false).ToListAsync();
    }
    public async Task<List<Participacion>> GetAllActiveDeclaracionAsync(int proceso, int? estadoDeclaracion)
    {
        if (estadoDeclaracion==0|| estadoDeclaracion ==null) {
            return await _context.Participaciones.Where(ep => ep.Eliminado == false && ep.IdDeclaracionNavigation.IdFormularioNavigation.IdProceso == proceso)

           .Include(a => a.IdDeclaracionNavigation)
           .Include(a => a.IdDeclaracionNavigation.IdCiudadNavigation)
           .Include(a => a.IdDeclaracionNavigation.IdCiudadNavigation.IdPaisNavigation)
           .Include(a => a.IdParentescoNavigation)
                 .Include(a => a.IdDeclaracionNavigation.IdFormularioNavigation)
           .ToListAsync();
        }
        else
        {
            return await _context.Participaciones.Where(ep => ep.Eliminado == false && ep.IdDeclaracionNavigation.IdFormularioNavigation.IdProceso == proceso && ep.IdDeclaracionNavigation.IdEstadoDeclaracion == estadoDeclaracion)

           .Include(a => a.IdDeclaracionNavigation)
           .Include(a => a.IdDeclaracionNavigation.IdCiudadNavigation)
           .Include(a => a.IdDeclaracionNavigation.IdCiudadNavigation.IdPaisNavigation)
           .Include(a => a.IdParentescoNavigation)
                 .Include(a => a.IdDeclaracionNavigation.IdFormularioNavigation)
           .ToListAsync();
        }
       
    }

    public async Task<List<Participacion>> GetAllAsync()
    {
        return await _context.Participaciones.ToListAsync();
    }

    public async Task<List<Participacion>> GetByIdDeclaracionAsync(int id)
    {
        return await _context.Participaciones
            .Include(a => a.IdPaisNavigation)
            .Include(a => a.IdDeclaracionNavigation)
            .Include(a => a.IdParentescoNavigation)
            .Where(ep => !ep.Eliminado && ep.IdDeclaracion == id).ToListAsync();
    }

    public async Task<Participacion> GetByIdAsync(int id)
    {
        return await _context.Participaciones.Where(ep => ep.IdParticipacion == id).FirstOrDefaultAsync();
    }

    public async Task<Participacion> UpdateAsync(Participacion entity)
    {
        _context.Participaciones.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
