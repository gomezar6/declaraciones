using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class SincronizacionRepository : IEntityRepository<Sincronizacion>
    {
        private readonly DeclaracionesContext _context;

        public SincronizacionRepository(DeclaracionesContext context)
        {
            _context = context;
        }

        public Task<Sincronizacion> AddAsync(Sincronizacion entity)
        {
            throw new NotImplementedException();
        }

        public Task<Sincronizacion> DeleteAsync(Sincronizacion entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Sincronizacion>> GetAllActiveAsync()
        {
            return await _context
                .Sincronizaciones
                .Include(s => s.SincronizacionDetalles)
                .Where(s => !s.Eliminado)
                .ToListAsync();
        }

        public async Task<List<Sincronizacion>> GetAllAsync()
        {
            return await _context
                .Sincronizaciones
                .Include(s => s.SincronizacionDetalles)
                .ToListAsync();
        }

        public async Task<Sincronizacion?> GetByIdAsync(int id)
        {
            return await _context
                .Sincronizaciones
                .Include(s => s.SincronizacionDetalles)
                .FirstOrDefaultAsync(s => s.IdSincronizacion == id);
        }

        public Task<Sincronizacion> UpdateAsync(Sincronizacion entity)
        {
            throw new NotImplementedException();
        }
    }
}
