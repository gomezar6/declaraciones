#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;
public class DisclaimersRepository : IEntityRepository<Disclaimer>
{
    private readonly DeclaracionesContext _context;
    public DisclaimersRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<Disclaimer> AddAsync(Disclaimer entity)
    {
        var result = await _context.Disclaimers.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdDisclaimer);
    }

    public async Task<List<Disclaimer>> GetAllAsync()
    {
        //.Where(_disclaimer => _disclaimer.IdProceso == 4).
        return await _context.Disclaimers.Include(d => d.IdProcesoNavigation).ToListAsync();
    }

    public async Task<List<Disclaimer>> GetAllActiveAsync()
    {
        return await _context.Disclaimers.Where(p => p.Eliminado == false).ToListAsync();
    }

    public async Task<Disclaimer> GetByIdAsync(int id)
    {
        return await _context.Disclaimers
            .Include(d => d.IdProcesoNavigation)
            .FirstOrDefaultAsync(p => p.IdDisclaimer == id);
    }

    public async Task<Disclaimer> GetByTextAsync(string titulo,int idProceso)
    {
        return await _context.Disclaimers
            .Include(d => d.IdProcesoNavigation).Where(a => a.IdProceso== idProceso )
            .FirstOrDefaultAsync(p => p.Titulo == titulo && p.Eliminado == false);
    }


    public async Task<Disclaimer> UpdateAsync(Disclaimer entity)
    {
        // TODO: Manejar errores
        _context.Disclaimers.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Disclaimer> DeleteAsync(Disclaimer entity)
    {
        _context.Disclaimers.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Disclaimers.AnyAsync(e => e.IdDisclaimer == id);
    }
}
