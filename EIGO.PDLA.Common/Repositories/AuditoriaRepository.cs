#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class AuditoriaRepository : IEntityRepository<Auditoria>
{
    private readonly DeclaracionesContext _context;

    public AuditoriaRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<Auditoria> AddAsync(Auditoria entity)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>

    public async Task<Auditoria> DeleteAsync(Auditoria entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exist(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exist(int IdParticipacion, int IdDeclaracion)
    {
        throw new NotImplementedException();
    }
    public async Task<List<Auditoria>> GetAllActiveAsync()
    {
        return await _context.Auditoria.ToListAsync();
    }

    public async Task<List<Auditoria>> GetAllAsync()
    {
        return await _context.Auditoria.ToListAsync();
    }

    public async Task<List<Auditoria>> GetByIdProcesoAsync(int id)
    {
        return await _context.Auditoria
            .Where(ep => ep.IdProceso == id).ToListAsync();
    }

    public async Task<Auditoria> GetByIdAsync(int id)
    {
        return await _context.Auditoria.Where(ep => ep.IdProceso == id).FirstOrDefaultAsync();
    }

    public async Task<Auditoria> UpdateAsync(Auditoria entity)
    {

        throw new NotImplementedException();
    }
}
