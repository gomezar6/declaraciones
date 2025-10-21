using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class FamiliarRepository : IEntityRepository<Familiar>
    {
        private readonly DeclaracionesContext _context;

        public FamiliarRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Familiar> AddAsync(Familiar entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Familiar> DeleteAsync(Familiar entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Familiares.AnyAsync(ep => ep.IdFamiliar == id);
        }

        public async Task<List<Familiar>> GetAllActiveAsync()
        {
            return await _context.Familiares.Where(ep => ep.Eliminado == false).ToListAsync();
        }

        public async Task<List<Familiar>> GetAllAsync()
        {
            return await _context.Familiares.ToListAsync();
        }
       
        public async Task<Familiar?> GetByIdAsync(int id)
        {
            return await _context.Familiares.Where(ep => ep.IdFamiliar == id).FirstOrDefaultAsync();
        }

        public async Task<List<Familiar>> GetByIdFuncionarioAsync(int id)
        {
            return await _context.Familiares.Where(ep => ep.IdFuncionario == id).ToListAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Familiar> UpdateAsync(Familiar entity)
        {
            throw new NotImplementedException();
        }
    }
}
