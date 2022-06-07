using Dapper;
using DapperExtensions;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System.Data;
using System.Linq.Expressions;
using Vesta.Ddd.Domain.Entities;
using Vesta.Ddd.Domain.Repositories;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Dapper.Domain.Repositories
{
    public abstract class DapperRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TKey>
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        protected IDbConnection DbConnection => GetDbConnection();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        public DapperRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            ArgumentNullException.ThrowIfNull(dbContextProvider, nameof(dbContextProvider));

            _dbContextProvider = dbContextProvider;

        }

        private IDbConnection GetDbConnection()
        {
            return AsyncContext.Run(async () =>
            {
                var dbContext = await _dbContextProvider.GetDbContextAsync();
                return dbContext.Database.GetDbConnection();
            });

        }

        public async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            DbConnection.Open();

            var query = (await DbConnection.GetListAsync<TEntity>(predicate)).AsQueryable();

            DbConnection.Close();

            return orderBy is null ?
                await query.ToListAsync() :
                await orderBy(query).ToListAsync();
        }

        public async ValueTask<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            DbConnection.Open();

            var entity = await DbConnection.GetAsync<TEntity>(id);

            DbConnection.Close();

            return entity;
        }

        public async Task InsertAsync(TEntity entity, /*bool autoSave = false,*/ CancellationToken cancellationToken = default)
        {

            DbConnection.Open();

            await DbConnection.InsertAsync(entity);

            DbConnection.Close();

        }

        public async Task DeleteAsync(TKey id,/* bool autoSave = false,*/ CancellationToken cancellationToken = default)
        {
            DbConnection.Open();

            await DbConnection.DeleteAsync<TEntity>(id);

            DbConnection.Close();
        }

        public async Task DeleteAsync(TEntity entity,/* bool autoSave = false,*/ CancellationToken cancellationToken = default)
        {

            await DeleteAsync(entity.As<IEntity<TKey>>().Id);
        }

        public async Task UpdateAsync(TEntity entity, /*bool autoSave = false,*/ CancellationToken cancellationToken = default)
        {
            DbConnection.Open();

            await DbConnection.UpdateAsync(entity);

            DbConnection.Close();
        }
    }
}