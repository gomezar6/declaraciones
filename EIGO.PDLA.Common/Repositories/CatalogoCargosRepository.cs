using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class CatalogoCargosRepository : IEntityRepository<CatalogoCargos>
    {
        private readonly DeclaracionesContext _context;

        public CatalogoCargosRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoCargos> AddAsync(CatalogoCargos entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoCargos> DeleteAsync(CatalogoCargos entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.CatalogoCargos.AnyAsync(ep => ep.id == id);
        }

        public async Task<List<CatalogoCargos>> GetAllActiveAsync()
        {
            return await _context.CatalogoCargos.OrderBy(ep => ep.Opcion).ToListAsync();
        }

        public async Task<List<CatalogoCargos>> GetAllAsync()
        {
            return await _context.CatalogoCargos.OrderBy(ep => ep.Opcion).ToListAsync();
        }
       
        public async Task<CatalogoCargos?> GetByIdAsync(int id)
        {
            return await _context.CatalogoCargos.Where(ep => ep.id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoCargos> UpdateAsync(CatalogoCargos entity)
        {
            throw new NotImplementedException();
        }
    }
}
