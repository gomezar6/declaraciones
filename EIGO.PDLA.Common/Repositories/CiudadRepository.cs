using EIGO.PDLA.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EIGO.PDLA.Common.Repositories
{
    public class CiudadRepository : IEntityRepository<Ciudad>
    {
        private readonly DeclaracionesContext _context;

        public CiudadRepository(DeclaracionesContext context)
        {
            _context = context;
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Ciudad> AddAsync(Ciudad entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Ciudad> DeleteAsync(Ciudad entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Ciudades.AnyAsync(ep => ep.IdCiudad == id);
        }

        public async Task<List<Ciudad>> GetCiudadByPais(int idpais)
        {
            return await _context.Ciudades.Where(ep => ep.IdPais == idpais && ep.IdPaisNavigation.Eliminado == false).OrderBy(ep => ep.NombreCiudad).ToListAsync();
        }

        public async Task<List<Ciudad>> GetAllActiveAsync()
        {
            return await _context.Ciudades.Where(ep => ep.Eliminado == false).OrderBy(ep => ep.NombreCiudad).ToListAsync();
        }

        public async Task<List<Ciudad>> GetAllAsync()
        {
            return await _context.Ciudades.OrderBy(ep => ep.NombreCiudad).ToListAsync();
        }

        public async Task<Ciudad?> GetByIdAsync(int id)
        {
            return await _context.Ciudades.Where(ep => ep.IdCiudad == id).FirstOrDefaultAsync();
        }
        /// <summary>
        /// NO IMPLEMENTADO / NO PERMITIDO <b>NO UTILIZAR</b>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Ciudad> UpdateAsync(Ciudad entity)
        {
            throw new NotImplementedException();
        }
    }
}
