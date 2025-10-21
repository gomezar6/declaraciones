#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EIGO.PDLA.Common.Repositories;

public class ProcesoRepository : IEntityRepository<Proceso>
{
    private readonly DeclaracionesContext _context;

    public ProcesoRepository(DeclaracionesContext context)
    {
        _context = context;
    }

    public async Task<Proceso> AddAsync(Proceso entity)
    {
        var result = await _context.Procesos.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdProceso);
    }

    public async Task<List<Proceso>> GetAllAsync()
    {
        return await _context.Procesos
            .Include(p => p.IdEstadoProcesoNavigation)
            .ToListAsync();
    }

    public async Task<List<Proceso>> GetAllActiveAsync()
    {
        return await _context.Procesos
            .Include(p => p.IdEstadoProcesoNavigation)
            .Include(p => p.Alerta.Where(a => !a.Eliminado))
            .Include(p => p.Formularios.Where(_formularios => !_formularios.Eliminado))
            .Include("Formularios.IdTipoDeclaracionNavigation")
            .Include(p => p.Disclaimers.Where(_disclaimer => !_disclaimer.Eliminado))
            .Where(p => !p.Eliminado).OrderByDescending(pf => pf.IdProceso)
            .ToListAsync();
    }

    public Task<List<Proceso>> GetAllActiveAsync(Expression<Func<Proceso, bool>> whereFunc)
    {
        IQueryable<Proceso> query = _context.Procesos.Where(pf => !pf.Eliminado).Where(whereFunc).OrderByDescending(pf => pf.IdProceso);
        return query.ToListAsync();

    }

    public async Task<List<Proceso>> GetAllActiveApartirEnProcesoAsync()
    {
        return await _context.Procesos
            .Include(p => p.IdEstadoProcesoNavigation)
            .Include(p => p.Alerta.Where(a => !a.Eliminado))
            .Include(p => p.Formularios.Where(_formularios => !_formularios.Eliminado))
            .Include("Formularios.IdTipoDeclaracionNavigation")
            .Include(p => p.Disclaimers.Where(_disclaimer => !_disclaimer.Eliminado))
            .Where(p => !p.Eliminado && p.IdEstadoProceso > 2)
            .ToListAsync();
    }

    public async Task<Proceso> GetByIdAsync(int id)
    {
        return await _context.Procesos
            .Include(p => p.IdEstadoProcesoNavigation)
            .Include(p => p.Alerta.Where(a => !a.Eliminado))
            .Include(p => p.Formularios.Where(_formularios => !_formularios.Eliminado))
            .Include("Formularios.IdTipoDeclaracionNavigation")
            .Include(p => p.Disclaimers.Where(_disclaimer => !_disclaimer.Eliminado))
            .FirstOrDefaultAsync(p => p.IdProceso == id);
    }

    public async Task<Proceso> UpdateAsync(Proceso entity)
    {
        // TODO: Manejar errores
        try
        {
            _context.Procesos.Update(entity);
            _ = await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return entity;
    }

    public async Task<Proceso> DeleteAsync(Proceso entity)
    {
        foreach (var alerta in entity.Alerta)
        {
            _context.Remove(alerta);
        }
        foreach (var formulario in entity.Formularios)
        {
            _context.Remove(formulario);
        }
        foreach (var disclaimer in entity.Disclaimers)
        {
            _context.Remove(disclaimer);
        }
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Procesos.AnyAsync(e => e.IdProceso == id);
    }
}
