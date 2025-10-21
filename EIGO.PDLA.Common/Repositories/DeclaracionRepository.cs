#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class DeclaracionRepository : IEntityRepository<Declaracion>
{
    private readonly DeclaracionesContext _context;
    public DeclaracionRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    public async Task<Declaracion> AddAsync(Declaracion entity)
    {
        var result = await _context.Declaraciones.AddAsync(entity);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(result.Entity.IdDeclaracion);
    }

    public async Task<Declaracion> DeleteAsync(Declaracion entity)
    {
        _context.Declaraciones.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Declaraciones.AnyAsync(e => e.IdDeclaracion == id);
    }

    public async Task<List<Declaracion>> GetAllActiveAsync()
    {
        return await _context.Declaraciones
           .Include(a => a.IdEstadoDeclaracionNavigation)
            .Where(a => !a.Eliminado).ToListAsync();
    }

    public async Task<List<Declaracion>> GetAllAsync()
    {
        return await _context.Declaraciones
            .Include(a => a.IdEstadoDeclaracionNavigation)
            .ToListAsync();
    }

    public async Task<Declaracion?> GetByIdAsync(int id)
    {
        return await _context.Declaraciones
            .Include(a => a.IdEstadoDeclaracionNavigation)
            .Include(a => a.IdFormularioNavigation)
            .FirstOrDefaultAsync(a => a.IdDeclaracion == id);
    }

    public async Task<List<Declaracion>> GetAllActivebyFuncionarioAsync(int id)
    {
        return await _context.Declaraciones
            .Include(a => a.IdEstadoDeclaracionNavigation)
            .Include(a => a.IdFormularioNavigation)
             .Include(a => a.IdFuncionarioNavigation)
            .Where(a => !a.Eliminado && a.IdFuncionario == id && !a.IdFormularioNavigation.Eliminado).OrderBy(pf => pf.IdFuncionarioNavigation.Nombres).ToListAsync();

    }

    public async Task<List<Declaracion>> GetAllActivebyProcesoAsync(int idProceso)
    {
        return await _context.Declaraciones
            .Include(a => a.IdEstadoDeclaracionNavigation)
            .Include(a => a.IdFormularioNavigation)
             .Include(a => a.IdFuncionarioNavigation)
            .Where(a => !a.Eliminado && a.IdFormularioNavigation.IdProceso == idProceso).OrderBy(pf => pf.IdFuncionarioNavigation.Nombres).ToListAsync();
    }

    public async Task<Declaracion> UpdateAsync(Declaracion entity)
    {
        // TODO: Manejar errores
        _context.Declaraciones.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
