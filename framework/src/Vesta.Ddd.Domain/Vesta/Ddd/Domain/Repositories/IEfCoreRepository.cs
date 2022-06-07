﻿using System.Linq.Expressions;
using Vesta.Ddd.Domain.Entities;

namespace Vesta.Ddd.Domain.Repositories
{
    public interface IEfCoreRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : IEntity<int>
    {
    }

    public interface IEfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {

        /// <summary>
        /// Find a entity that matches the identifier 
        /// </summary>
        /// <param name="filter">ID</param>
        /// <param name="orderBy">ID</param>
        /// <param name="includeProperties">ID</param>
        /// <returns>Entities</returns>
        public Task<List<TEntity>> GetListAsync(
                    Expression<Func<TEntity, bool>> predicate = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    params string[] includeProperties);


        /// <summary>
        /// Insert a entity
        /// </summary>
        /// <param name="entity"></param>
        Task InsertAsync(TEntity entity, bool autoSave, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes the entity that matches the identifier 
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(TKey id, bool autoSave, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove a entity 
        /// </summary>
        /// <param name="id">Entity</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(TEntity entity, bool autoSave, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="id">Entity</param>
        /// <param name="autoSave">True, para save changes. Otherwise, False.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateAsync(TEntity entity, bool autoSave, CancellationToken cancellationToken = default);

    }

}
