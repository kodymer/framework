using Ardalis.Specification;
using CommunityToolkit.Diagnostics;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.Ddd.Domain.Repositories;
using CompanyName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.EntityFrameworkCore.Repositories
{
    public class EfCoreRepository<TDbContext, TEntity> : EfCoreReadOnlyRepository<TDbContext, TEntity>, IRepository<TEntity>
        where TEntity : class, IAggregateRoot
        where TDbContext : CompanyNameDbContextBase<TDbContext>
    {

        public EfCoreRepository(TDbContext dbContext)
            : base(dbContext)
        {
        }

        public EfCoreRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
            : base(dbContext, specificationEvaluator)
        {
        }

        /// <inheritdoc/>
        public override Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            return AddAsync(entity, false, cancellationToken);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            if (save)
            {
                return await base.AddAsync(entity, cancellationToken);
            }

            var result = await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        /// <inheritdoc/>
        public override Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            return AddRangeAsync(entities, false, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            if (save)
            {
                return await base.AddRangeAsync(entities, cancellationToken);
            }

            await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

            return entities;
        }

        /// <inheritdoc/>
        public override Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            return UpdateAsync(entity, false, cancellationToken);
        }

        public virtual Task<int> UpdateAsync(TEntity entity, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            if (save)
            {
                return base.UpdateAsync(entity, cancellationToken);
            }

            DbContext.Set<TEntity>().Update(entity);

            return Task.FromResult(0);
        }

        /// <inheritdoc/>
        public override Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            return UpdateRangeAsync(entities, false, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            if (save)
            {
                return base.UpdateRangeAsync(entities, cancellationToken);
            }

            DbContext.Set<TEntity>().UpdateRange(entities);

            return Task.FromResult(0); ;
        }

        /// <inheritdoc/>
        public override Task<int> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            return DeleteAsync(entity, false, cancellationToken);
        }

        public virtual Task<int> DeleteAsync(TEntity entity, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entity);

            if (save)
            {
                return base.DeleteAsync(entity, cancellationToken);
            }

            DbContext.Set<TEntity>().Remove(entity);

            return Task.FromResult(0);
        }

        public virtual Task<int> DeleteAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(id);

            return DeleteAsync(id, false, cancellationToken);
        }

        public virtual async Task<int> DeleteAsync<TKey>(TKey id, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(id);

            var entity = await GetByIdAsync(id, cancellationToken);
            return await DeleteAsync(entity, save, cancellationToken);
        }

        /// <inheritdoc/>
        public override Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            return DeleteRangeAsync(entities, false, cancellationToken);
        }

        /// <inheritdoc/>
        public virtual Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(entities);
            Guard.IsNotEmpty(entities.ToList());

            if (save)
            {
                return base.DeleteRangeAsync(entities, cancellationToken);
            }

            DbContext.Set<TEntity>().RemoveRange(entities);

            return Task.FromResult(0);
        }

        /// <inheritdoc/>
        public override Task<int> DeleteRangeAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(specification);

            return DeleteRangeAsync(specification, false, cancellationToken);
        }

        public virtual Task<int> DeleteRangeAsync(ISpecification<TEntity> specification, bool save, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(specification);

            if (save)
            {
                return base.DeleteRangeAsync(specification, cancellationToken);
            }

            var query = ApplySpecification(specification);
            DbContext.Set<TEntity>().RemoveRange(query);

            return Task.FromResult(0);
        }
    }
}
