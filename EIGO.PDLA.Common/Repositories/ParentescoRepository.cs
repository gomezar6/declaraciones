using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class ParentescoRepository : IEntityRepository<Parentesco>
    {
        private readonly DeclaracionesContext _context;

        public ParentescoRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Parentesco> AddAsync(Parentesco entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Parentesco> DeleteAsync(Parentesco entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Parentescos.AnyAsync(ep => ep.IdParentesco == id);
        }

        public async Task<List<Parentesco>> GetAllActiveAsync()
        {
            return await _context.Parentescos.Where(ep => ep.Eliminado == false).ToListAsync();
        }

        public async Task<List<Parentesco>> GetAllAsync()
        {
            return await _context.Parentescos.ToListAsync();
        }

        public async Task<Parentesco?> GetByIdAsync(int id)
        {
            return await _context.Parentescos.Where(ep => ep.IdParentesco == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Parentesco> UpdateAsync(Parentesco entity)
        {
            throw new NotImplementedException();
        }
    }
}
