using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class PaisRepository : IEntityRepository<Pais>
    {
        private readonly DeclaracionesContext _context;

        public PaisRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Pais> AddAsync(Pais entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Pais> DeleteAsync(Pais entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Paises.AnyAsync(ep => ep.IdPais == id);
        }

        public async Task<List<Pais>> GetAllActiveAsync()
        {
            return await _context.Paises.Where(ep => ep.Eliminado == false).OrderBy(ep=> ep.NombrePais).ToListAsync();
        }

        public async Task<List<Pais>> GetAllAsync()
        {
            return await _context.Paises.OrderBy(ep => ep.NombrePais).ToListAsync();
        }
       
        public async Task<Pais?> GetByIdAsync(int id)
        {
            return await _context.Paises.Where(ep => ep.IdPais == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Pais> UpdateAsync(Pais entity)
        {
            throw new NotImplementedException();
        }
    }
}
