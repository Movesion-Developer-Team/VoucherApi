using System.Linq.Expressions;

namespace Core.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetAsync(int id);
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);


        Task AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
