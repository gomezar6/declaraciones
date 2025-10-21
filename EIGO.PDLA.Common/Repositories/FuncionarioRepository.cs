#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class FuncionarioRepository : IEntityRepository<Funcionario>
{
    private readonly DeclaracionesContext _context;

    public FuncionarioRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    public async Task<Funcionario> AddAsync(Funcionario entity)
    {
        var result = await _context.Funcionarios.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdFuncionario);
    }

    public async Task<Funcionario> DeleteAsync(Funcionario entity)
    {
        _context.Funcionarios.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Funcionarios.AnyAsync(e => e.IdFuncionario == id);
    }

    public async Task<List<Funcionario>> GetAllActiveAsync()
    {
        return await _context.Funcionarios.Where(f => !f.Eliminado).ToListAsync();
    }

    public async Task<List<Funcionario>> GetAllActiveAsync(Func<Funcionario, bool> query)
    {
        return await _context.Funcionarios.Where(f => query(f) && !f.Eliminado).ToListAsync();
    }

    public async Task<List<Funcionario>> GetAllAsync()
    {
        return await _context.Funcionarios.ToListAsync();
    }

    public async Task<Funcionario> GetByIdAsync(int id)
    {
        return await _context.Funcionarios
            .Where(p => !p.Eliminado)
            .FirstOrDefaultAsync(a => a.IdFuncionario == id);
    }

    public async Task<Funcionario?> GetByEmailAsync(string id)
    {
        return await _context.Funcionarios
            .Where(p => !p.Eliminado)
            .FirstOrDefaultAsync(a => a.Email == id);
    }

      public async Task<Funcionario?> GetByCUPAsync(int Cup)
    {
        return await _context.Funcionarios
            .Where(p => !p.Eliminado)
            .FirstOrDefaultAsync(a => a.Cup == Cup);
    }

    public async Task<Funcionario> UpdateAsync(Funcionario entity)
    {
        // TODO: Manejar errores
        _context.Funcionarios.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
