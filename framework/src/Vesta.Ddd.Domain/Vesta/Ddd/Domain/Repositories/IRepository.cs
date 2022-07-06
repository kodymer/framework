using System.Linq.Expressions;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.Ddd.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// Get queryable
        /// </summary>
        /// <returns>Queryable</returns>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Find a entity that matches the identifier. <see cref="TEntity"/>
        /// </summary>
        /// <param name="filter">ID</param>
        /// <param name="orderBy">ID</param>
        /// <param name="includeProperties">ID</param>
        /// <returns>Entities</returns>
        public Task<List<TEntity>> GetListAsync(
                    Expression<Func<TEntity, bool>> predicate = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    params string[] includeProperties);
    }

    public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Get a entity. <see cref="TEntity"/>
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entity</returns>
        ValueTask<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {

        /// <summary>
        /// Insert a entity. <see cref="TEntity"/>
        /// </summary>
        /// <param name="entity"></param>
        Task InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the entities that matches the identifier. <see cref="TEntity"/>
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove a entity. <see cref="TEntity"/> 
        /// </summary>
        /// <param name="id">Entity</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a entity. <see cref="TEntity"/>
        /// </summary>
        /// <param name="id">Entity</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    }

    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : IEntity<int>
    {

    }
}
