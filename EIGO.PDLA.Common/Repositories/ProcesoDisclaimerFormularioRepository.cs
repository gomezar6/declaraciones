#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class ProcesoDisclaimerFormularioRepository : IEntityRepository<ProcesoDisclaimerFormulario>
{
    private readonly DeclaracionesContext _context;
    public ProcesoDisclaimerFormularioRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    public async Task<ProcesoDisclaimerFormulario> AddAsync(ProcesoDisclaimerFormulario entity)
    {
        var result = await _context.ProcesoDisclaimerFormularios.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdFormulario);
    }

    public async Task<ProcesoDisclaimerFormulario> DeleteAsync(ProcesoDisclaimerFormulario entity)
    {
        _context.ProcesoDisclaimerFormularios.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.ProcesoDisclaimerFormularios.AnyAsync(e => e.IdFormulario == id);
    }

    public async Task<List<ProcesoDisclaimerFormulario>> GetAllActiveAsync()
    {
        return await _context.ProcesoDisclaimerFormularios
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdFormularioNavigation)
            .Include(a => a.IdDisclaimerNavigation)
            .Where(a => !a.Eliminado)
            .ToListAsync();
    }

    public async Task<List<ProcesoDisclaimerFormulario>> GetAllAsync()
    {
        return await _context.ProcesoDisclaimerFormularios.OrderByDescending(pf => pf.Id)
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdFormularioNavigation)
            .Include(a => a.IdDisclaimerNavigation)
            .ToListAsync();
    }

    public async Task<List<ProcesoDisclaimerFormulario>> GetbyProcesoAsync(int idproceso)
    {
        return await _context.ProcesoDisclaimerFormularios.OrderBy(pf => pf.Id ).Where(a=>a.IdProceso== idproceso && a.Eliminado == false && a.IdFormularioNavigation.Eliminado==false)
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdFormularioNavigation)
            .Include(a => a.IdDisclaimerNavigation)
            .ToListAsync();
    }
    public async Task<ProcesoDisclaimerFormulario> GetByIdAsync(int id)
    {
        return await _context.ProcesoDisclaimerFormularios
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdFormularioNavigation)
            .Include(a => a.IdDisclaimerNavigation)
            .FirstOrDefaultAsync(a => a.IdFormulario == id);
    }
    public async Task<ProcesoDisclaimerFormulario> GetByAllPKAsync(ProcesoDisclaimerFormulario entity)
    {
        return await _context.ProcesoDisclaimerFormularios
            .FirstOrDefaultAsync(a => a.IdProceso == entity.IdProceso && a.IdDisclaimer == entity.IdDisclaimer && a.IdFormulario == entity.IdFormulario && a.Eliminado == false);
    }
    public async Task<List<ProcesoDisclaimerFormulario>> GetByIdListAsync(int id)
    {
        return await _context.ProcesoDisclaimerFormularios
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdFormularioNavigation)
            .Include(a => a.IdDisclaimerNavigation)
            .Where(a => !a.Eliminado && a.IdFormulario == id).ToListAsync();
    }

    public async Task<ProcesoDisclaimerFormulario> UpdateAsync(ProcesoDisclaimerFormulario entity)
    {
        // TODO: Manejar errores
        _context.ProcesoDisclaimerFormularios.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
