namespace EIGO.PDLA.Common.Repositories
{
    public interface IEntityRepository<T>
    {
        /// <summary>
        /// Agregar entidad
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        /// <returns>Entidad insertada</returns>
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllActiveAsync();
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);

        Task<bool> Exist(int id);
    }
}
