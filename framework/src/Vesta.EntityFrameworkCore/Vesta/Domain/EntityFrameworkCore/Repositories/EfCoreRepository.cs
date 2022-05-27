using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vesta.Ddd.Domain.Entities;
using Vesta.Ddd.Domain.Repositories;
using Vesta.EntityFrameworkCore;


namespace Vesta.Domain.EntityFrameworkCore.Repositories
{
   
    public abstract class EfCoreRepository<TDbContext, TEntity>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {

        protected IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            ArgumentNullException.ThrowIfNull(dbContextProvider, nameof(dbContextProvider));

            _dbContextProvider = dbContextProvider;
        }

        public Task<TDbContext> GetDbContextAsync()
        {
            return _dbContextProvider.GetDbContextAsync();
        }

        public async virtual Task<List<TEntity>> GetListAsync(
                    Expression<Func<TEntity, bool>> predicate = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    params string[] includeProperties)
        {
            var dbContext = await GetDbContextAsync();
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return orderBy is null ? 
                await query.ToListAsync() :
                await orderBy(query).ToListAsync();
            
        }

        protected virtual async Task<DbSet<TEntity>> GetDbSetAsync()
        {
            return (await GetDbContextAsync()).Set<TEntity>();
        }
    }

    /// <summary>
    /// Enttity Framework repository base
    /// </summary>
    public abstract class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>, IRepository<TEntity, TKey> 
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TKey>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async virtual ValueTask<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var entity = await dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            await dbSet.AddAsync(entity);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async virtual Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var entity = await dbSet.FindAsync(new object[] { id }, cancellationToken);
            if(entity is null)
            {
                return;
            }

            await DeleteAsync(entity, cancellationToken: cancellationToken);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async virtual Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();

            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }

        }

        public async virtual Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            dbSet.Attach(entity);

            dbContext.Entry(entity).State = EntityState.Modified;

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
