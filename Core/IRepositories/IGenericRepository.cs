using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetAsync(int id);
        Task<IQueryable<TEntity>> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Update(TEntity entity);


        Task<int?> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(int id);
        Task<bool> AnyAsync();
        Task<bool> ExistsAsync(int id);
    }
}
