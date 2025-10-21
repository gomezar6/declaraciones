#nullable disable
using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories;

public class FormularioRepository : IEntityRepository<Formulario>
{
    private readonly DeclaracionesContext _context;
    public FormularioRepository(DeclaracionesContext context)
    {
        _context = context;
    }
    public async Task<Formulario> AddAsync(Formulario entity)
    {
        var result = await _context.Formularios.AddAsync(entity);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(result.Entity.IdFormulario);
    }

    public async Task<Formulario> DeleteAsync(Formulario entity)
    {
        _context.Formularios.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Formularios.AnyAsync(e => e.IdFormulario == id);
    }

    public async Task<bool> ExistFormulariobyType(int id, int idTipoDeclaracion)
    {
        return await _context.Formularios.AnyAsync(e => e.IdFormulario == id && e.IdTipoDeclaracion == idTipoDeclaracion);
    }

    public async Task<List<Formulario>> GetAllActiveAsync()
    {
        return await _context.Formularios
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdTipoDeclaracionNavigation)
            .Where(a => !a.Eliminado).ToListAsync();
    }

    public async Task<List<Formulario>> GetAllAsync()
    {
        return await _context.Formularios
            .Include(a => a.IdProcesoNavigation)
            .Include(a => a.IdTipoDeclaracionNavigation)
            .ToListAsync();
    }

    public async Task<Formulario> GetByIdAsync(int id)
    {
        return await _context.Formularios
            .Include(a => a.IdProcesoNavigation)
                    .Include(a => a.IdProcesoNavigation.IdEstadoProcesoNavigation)
            .Include(a => a.IdTipoDeclaracionNavigation)
            .Include(a => a.ProcesoDisclaimerFormulario.Where(p => p.Eliminado == false))

            .Include("ProcesoDisclaimerFormulario.IdDisclaimerNavigation")
            .FirstOrDefaultAsync(a => a.IdFormulario == id);
    }

    public async Task<Formulario> UpdateAsync(Formulario entity)
    {
        // TODO: Manejar errores
        _context.Formularios.Update(entity);
        _ = await _context.SaveChangesAsync();
        return entity;
    }
}
