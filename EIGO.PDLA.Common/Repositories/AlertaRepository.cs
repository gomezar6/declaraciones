using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace EIGO.PDLA.Common.Repositories;

public class AlertaRepository : IEntityRepository<Alerta>
{
    private readonly DeclaracionesContext _context;
    public AlertaRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<Alerta> AddAsync(Alerta entity)
    {
        var result = await _context.Alertas.AddAsync(entity);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(result.Entity.IdAlerta);
    }

    public async Task<List<Alerta>> GetAllAsync()
    {
        return await _context.Alertas
            .Include(a => a.IdProcesoNavigation)
            .ToListAsync();
    }

    public async Task<List<Alerta>> GetAllActiveAsync()
    {
        return await _context.Alertas
            .Include(a => a.IdProcesoNavigation)
            .Where(a => !a.Eliminado).ToListAsync();
    }

    public async Task<Alerta?> GetByIdAsync(int id)
    {
        return await _context.Alertas
            .Include(a => a.IdProcesoNavigation)
            .FirstOrDefaultAsync(a => a.IdAlerta == id);
    }
    public async Task<Alerta?> GetByAlertaDiligenciadaProcesoAsync(int id)
    {
        return await _context.Alertas
            .Include(a => a.IdProcesoNavigation).Where(a=>a.IdProceso == id)
            .FirstOrDefaultAsync(a => a.Diligenciado == true && a.Estatus == true);
    }

    public async Task<Alerta> UpdateAsync(Alerta entity)
    {
        // TODO: Manejar errores
        _context.Alertas.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Alerta> DeleteAsync(Alerta entity)
    {
        _context.Alertas.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Alertas.AnyAsync(e => e.IdAlerta == id);
    }
}
