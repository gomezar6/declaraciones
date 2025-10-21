#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class FuncionarioNacionalidadRepository : IEntityRepository<FuncionarioNacionalidad>
{
    private readonly DeclaracionesContext _context;

    public FuncionarioNacionalidadRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<FuncionarioNacionalidad> AddAsync(FuncionarioNacionalidad entity)
    {
        var result = await _context.FuncionarioNacionalidad.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.Id);
    }

    public async Task<FuncionarioNacionalidad> DeleteAsync(FuncionarioNacionalidad entity)
    {
        _context.FuncionarioNacionalidad.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.FuncionarioNacionalidad.AnyAsync(ep => ep.Id == id);
    }
    public async Task<bool> ExistbyDeclaracion(int IdDeclaracion, int Nacionalidad)
    {
        return await _context.FuncionarioNacionalidad.AnyAsync(ep => ep.IdDeclaracion == IdDeclaracion && ep.Nacionalidad == Nacionalidad && ep.Eliminado == false);
    }

    public async Task<List<FuncionarioNacionalidad>> GetAllActiveAsync()
    {
        return await _context.FuncionarioNacionalidad.Where(ep => ep.Eliminado == false).ToListAsync();
    }

    public async Task<List<FuncionarioNacionalidad>> GetAllAsync()
    {
        return await _context.FuncionarioNacionalidad.ToListAsync();
    }

    public async Task<FuncionarioNacionalidad> GetByIdAsync(int id)
    {
        return await _context.FuncionarioNacionalidad.Where(ep => ep.Id == id).FirstOrDefaultAsync();
    }
    public async Task<FuncionarioNacionalidad> GetListByCamposAsync(FuncionarioNacionalidad entity)
    {
        return await _context.FuncionarioNacionalidad.FirstOrDefaultAsync(ep => ep.IdFuncionario == entity.IdFuncionario && ep.IdDeclaracion == entity.IdDeclaracion && ep.Nacionalidad == entity.Nacionalidad &&  ep.Eliminado == false);
    }
    public async Task<List<FuncionarioNacionalidad>> GetListByIdAsync(int IdFuncionario, int IdDeclaracion)
    {
        return await _context.FuncionarioNacionalidad
            .Include(a => a.PaisNavigation)
            .Where(ep => ep.IdFuncionario == IdFuncionario && ep.Eliminado == false && ep.IdDeclaracion== IdDeclaracion).ToListAsync();
    }

    public async Task<List<FuncionarioNacionalidad>> GetListByDeclaracionFuncionarioAsync(int IdFuncionario, int idDeclaracion)
    {
        return await _context.FuncionarioNacionalidad
            .Include(a => a.PaisNavigation)
            .Where(ep => ep.IdFuncionario == IdFuncionario && ep.IdDeclaracion == idDeclaracion && ep.Eliminado == false).ToListAsync();
    }

    /// <summary>
    /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<FuncionarioNacionalidad> UpdateAsync(FuncionarioNacionalidad entity)
    {
        throw new NotImplementedException();
    }
}
