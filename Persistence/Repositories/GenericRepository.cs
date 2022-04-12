using System.Linq.Expressions;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;


        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();

        }

        public void Update(TEntity entity)
        {
            
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task AddAsync(TEntity entity)
        {
            if (DbSet.Contains(entity) == false)
            {

                await DbSet.AddAsync(entity);

            }
            else
            {
                throw new InvalidOperationException("Current entity is already existing in Database");
            }
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                DbSet.AddRange(entities);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await DbSet.FindAsync(id) != null;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (await DbSet.Where(predicate).AnyAsync())
            {
                return await DbSet.Where(predicate).Select(e => e).ToListAsync();
            }

            throw new NullReferenceException("No requested entities in database");
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await DbSet.FindAsync(id) ?? throw new NullReferenceException();
        }

        public Task<IQueryable<TEntity>> GetAll()
        {
            var dbSet = DbSet.AsQueryable() ?? throw new NullReferenceException();
            return Task.FromResult(dbSet);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {

                if (!ExistsAsync(id).Result) return false;

                var currentEntityTask = await DbSet.FindAsync(id);
                DbSet.Remove(currentEntityTask!);

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
