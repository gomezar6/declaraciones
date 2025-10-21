using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class EstadoDeclaracionRepository : IEntityRepository<EstadoDeclaracion>
    {
        private readonly DeclaracionesContext _context;

        public EstadoDeclaracionRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<EstadoDeclaracion> AddAsync(EstadoDeclaracion entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<EstadoDeclaracion> DeleteAsync(EstadoDeclaracion entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.EstadoDeclaraciones.AnyAsync(ep => ep.IdEstado == id);
        }

        public async Task<List<EstadoDeclaracion>> GetAllActiveAsync()
        {
            return await _context.EstadoDeclaraciones.Where(ep => !ep.Eliminado).ToListAsync();
        }

        public async Task<List<EstadoDeclaracion>> GetAllAsync()
        {
            return await _context.EstadoDeclaraciones.ToListAsync();
        }

        public async Task<EstadoDeclaracion?> GetByIdAsync(int id)
        {
            return await _context.EstadoDeclaraciones.Where(ep => ep.IdEstado == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<EstadoDeclaracion> UpdateAsync(EstadoDeclaracion entity)
        {
            throw new NotImplementedException();
        }
    }
}
