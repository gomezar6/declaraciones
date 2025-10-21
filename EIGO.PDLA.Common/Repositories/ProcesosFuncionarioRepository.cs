#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EIGO.PDLA.Common.Repositories;
public class ProcesosFuncionarioRepository : IEntityRepository<ProcesosFuncionario>
{
    private readonly DeclaracionesContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProcesosFuncionarioRepository(DeclaracionesContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProcesosFuncionario> AddAsync(ProcesosFuncionario entity)
    {
        var result = await _context.ProcesosFuncionarios.AddAsync(entity);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<ProcesosFuncionario> DeleteAsync(ProcesosFuncionario entity)
    {
        var result = _context.ProcesosFuncionarios.Remove(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public Task<bool> Exist(int id)
    {
        return Task.FromResult(false);
    }

    public Task<List<ProcesosFuncionario>> GetAllActiveAsync()
    {
        return _context.ProcesosFuncionarios.Where(pf => !pf.Eliminado).ToListAsync();
    }

    public Task<List<ProcesosFuncionario>> GetAllActiveAsync(Expression<Func<ProcesosFuncionario, bool>> whereFunc)
    {
        IQueryable<ProcesosFuncionario> query = _context.ProcesosFuncionarios.Where(pf => !pf.Eliminado).Where(whereFunc);
        return query.ToListAsync();

    }

    public Task<List<ProcesosFuncionario>> GetAllActiveByProcesoAsync(int idProceso)
    {
        return _context.ProcesosFuncionarios
            .Include(a => a.IdFuncionarioNavigation)
            .Where(pf => !pf.Eliminado && pf.IdProceso == idProceso).OrderBy(pf => pf.IdFuncionarioNavigation.Nombres).ToListAsync();
    }

    public Task<List<ProcesosFuncionario>> GetAllActiveByProcesoFuncionarioAsync(int idProceso, int idfuncionario)
    {
        return _context.ProcesosFuncionarios
            .Include(a => a.IdFuncionarioNavigation)
            .Include(a => a.IdProcesoNavigation)
            .Where(pf => !pf.Eliminado && pf.IdProceso == idProceso && pf.IdFuncionario == idfuncionario).ToListAsync();
    }

    public Task<List<ProcesosFuncionario>> GetAllActiveByFuncionarioAsync(int idfuncionario)
    {
        return _context.ProcesosFuncionarios
            .Include(a => a.IdFuncionarioNavigation)
            .Include(a => a.IdProcesoNavigation)
                .Include(a => a.IdProcesoNavigation.IdEstadoProcesoNavigation)
            .Where(pf => pf.Eliminado == false && pf.IdFuncionario == idfuncionario && pf.IdProcesoNavigation.IdEstadoProceso > 2).OrderByDescending(pf => pf.IdProcesoNavigation.FechaInicio).ToListAsync();
    }

    public Task<List<ProcesosFuncionario>> GetAllAsync()
    {
        return _context.ProcesosFuncionarios.ToListAsync();
    }

    public Task<ProcesosFuncionario> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProcesosFuncionario> UpdateAsync(ProcesosFuncionario entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddRange(int IdProceso, List<Funcionario> Funcionarios)
    {
        try
        {
            await _context.ProcesosFuncionarios.AddRangeAsync(Funcionarios.Select(f => new ProcesosFuncionario
            {
                IdProceso = IdProceso,
                IdFuncionario = f.IdFuncionario,
                Creado = DateTime.UtcNow,
                CreadoPor = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "PDLA APP",
                Modificado = DateTime.UtcNow,
                ModificadoPor = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "PDLA APP"
            }));
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Notify error
            return false;
        }
    }

    public async Task<bool> DeleteRange(List<ProcesosFuncionario> procesosFuncionarios)
    {
        try
        {
            _context.ProcesosFuncionarios.RemoveRange(procesosFuncionarios);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Notify error
            return false;
        }
    }
}
