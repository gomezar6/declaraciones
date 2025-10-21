using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class CatalogoAniosRepository : IEntityRepository<CatalogoAnios>
    {
        private readonly DeclaracionesContext _context;

        public CatalogoAniosRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoAnios> AddAsync(CatalogoAnios entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoAnios> DeleteAsync(CatalogoAnios entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.CatalogoAnios.AnyAsync(ep => ep.Id == id);
        }

        public async Task<List<CatalogoAnios>> GetAllActiveAsync()
        {
            DateTime fechaHoy = DateTime.Today;
            return await _context.CatalogoAnios.Where(ep => ep.Anio <= fechaHoy.Year).OrderByDescending(ep => ep.Anio).ToListAsync();
        }

        public async Task<List<CatalogoAnios>> GetAllAsync()
        {
            return await _context.CatalogoAnios.OrderBy(ep => ep.Anio).ToListAsync();
        }
       
        public async Task<CatalogoAnios?> GetByIdAsync(int id)
        {
            return await _context.CatalogoAnios.Where(ep => ep.Id == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CatalogoAnios> UpdateAsync(CatalogoAnios entity)
        {
            throw new NotImplementedException();
        }
    }
}
