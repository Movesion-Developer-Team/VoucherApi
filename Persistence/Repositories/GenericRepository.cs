using System.Linq.Expressions;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly DbContext Context;


        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        public void Update(TEntity entity)
        {
            
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int?> AddAsync(TEntity entity)
        {

            if (Context.Set<TEntity>().Contains(entity) == false)
            {

                var entry = await Context.Set<TEntity>().AddAsync(entity);
                await Context.SaveChangesAsync();
                var id = entry.CurrentValues.GetValue<int?>("Id");

                return id;

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
                Context.Set<TEntity>().AddRange(entities);
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
            return await Context.Set<TEntity>().FindAsync(id) != null;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (Context.Set<TEntity>().Where(predicate).Any())
            {
                return Context.Set<TEntity>().Where(predicate).Select(e => e);
            }

            throw new NullReferenceException("No requested entities in database");


        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id) ?? throw new NullReferenceException();
        }

        public Task<IQueryable<TEntity>> GetAll()
        {
            var entities = Context.Set<TEntity>().AsQueryable();
            if (!entities.Any())
            {
                throw new NullReferenceException("No entities in Db");
            }
            return Task.FromResult(entities);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {

                if (!ExistsAsync(id).Result) return false;

                var currentEntityTask = await Context.Set<TEntity>().FindAsync(id);
                Context.Set<TEntity>().Remove(currentEntityTask!);

                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AnyAsync()
        {
            return await Context.Set<TEntity>().AnyAsync();
        }

        public async Task Complete()
        {
            await Context.SaveChangesAsync();
        }
    }
}
