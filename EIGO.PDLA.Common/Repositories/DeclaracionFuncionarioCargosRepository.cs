#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class DeclaracionFuncionarioCargosRepository : IEntityRepository<DeclaracionFuncionarioCargos>
{
    private readonly DeclaracionesContext _context;
    public DeclaracionFuncionarioCargosRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    public async Task<DeclaracionFuncionarioCargos> AddAsync(DeclaracionFuncionarioCargos entity)
    {
        var result = await _context.DeclaracionFuncionarioCargos.AddAsync(entity);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(result.Entity.IdDeclaracion);
    }

    public async Task<DeclaracionFuncionarioCargos> DeleteAsync(DeclaracionFuncionarioCargos entity)
    {
        _context.DeclaracionFuncionarioCargos.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.DeclaracionFuncionarioCargos.AnyAsync(e => e.IdDeclaracion == id);
    }

    public async Task<bool> Exist(int id, int idFuncionario)
    {
        return await _context.DeclaracionFuncionarioCargos.AnyAsync(e => e.IdDeclaracion == id && e.IdFuncionario == idFuncionario);
    }

    public async Task<List<DeclaracionFuncionarioCargos>> GetAllActiveAsync()
    {
        return await _context.DeclaracionFuncionarioCargos
            .Include(a => a.IdFuncionarioNavigation)
            .Where(a => !a.Eliminado)
            .ToListAsync();
    }

    public async Task<List<DeclaracionFuncionarioCargos>> GetAllAsync()
    {
        return await _context.DeclaracionFuncionarioCargos
            .Include(a => a.IdFuncionarioNavigation)
            .ToListAsync();
    }

    public async Task<DeclaracionFuncionarioCargos> GetByIdAsync(int id)
    {
        return await _context.DeclaracionFuncionarioCargos
            .Include(a => a.IdFuncionarioNavigation)
            .FirstOrDefaultAsync(a => a.IdDeclaracion == id);
    }

    public async Task<DeclaracionFuncionarioCargos> UpdateAsync(DeclaracionFuncionarioCargos entity)
    {
        // TODO: Manejar errores
        _context.DeclaracionFuncionarioCargos.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
