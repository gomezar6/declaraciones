#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class EstadoProcesoRepository : IEntityRepository<EstadoProceso>
{
    private readonly DeclaracionesContext _context;

    public EstadoProcesoRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<EstadoProceso> AddAsync(EstadoProceso entity)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<EstadoProceso> DeleteAsync(EstadoProceso entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.EstadoProcesos.AnyAsync(ep => ep.IdEstadoProceso == id);
    }

    public async Task<List<EstadoProceso>> GetAllActiveAsync()
    {
        return await _context.EstadoProcesos.Where(ep => !ep.Eliminado).ToListAsync();
    }

    public async Task<List<EstadoProceso>> GetAllAsync()
    {
        return await _context.EstadoProcesos.ToListAsync();
    }

    public async Task<EstadoProceso> GetByIdAsync(int id)
    {
        return await _context.EstadoProcesos.Where(ep => ep.IdEstadoProceso == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<EstadoProceso> UpdateAsync(EstadoProceso entity)
    {
        throw new NotImplementedException();
    }
}
