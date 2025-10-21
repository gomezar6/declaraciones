using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class TipoDeclaracionRepository : IEntityRepository<TipoDeclaracion>
    {
        private readonly DeclaracionesContext _context;

        public TipoDeclaracionRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TipoDeclaracion> AddAsync(TipoDeclaracion entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TipoDeclaracion> DeleteAsync(TipoDeclaracion entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.TipoDeclaraciones.AnyAsync(ep => ep.IdTipo == id);
        }

        public async Task<List<TipoDeclaracion>> GetAllActiveAsync()
        {
            return await _context.TipoDeclaraciones.Where(ep => ep.Eliminado == false).ToListAsync();
        }

        public async Task<List<TipoDeclaracion>> GetAllAsync()
        {
            return await _context.TipoDeclaraciones.ToListAsync();
        }

        public async Task<TipoDeclaracion?> GetByIdAsync(int id)
        {
            return await _context.TipoDeclaraciones.Where(ep => ep.IdTipo == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TipoDeclaracion> UpdateAsync(TipoDeclaracion entity)
        {
            throw new NotImplementedException();
        }
    }
}
