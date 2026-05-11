using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyName.Ddd.Domain.Common.Filters;
using CompanyName.Ddd.Domain.Common.Pagination;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.Ddd.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.EntityFrameworkCore.Repositories
{
    public class EfCoreReadOnlyRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IReadOnlyRepository<TEntity>
            where TEntity : class, IEntity
            where TDbContext : CompanyNameDbContextBase<TDbContext>
    {

        protected internal IConfigurationProvider ConfigurationProvider { get; internal set; }

        public EfCoreReadOnlyRepository(TDbContext dbContext)
          : base(dbContext)
        {
        }

        public EfCoreReadOnlyRepository(TDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
            : base(dbContext, specificationEvaluator)
        {
        }


        public Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
                => DbContext.Set<TEntity>().FindAsync(keyValues, cancellationToken).AsTask();

        public async Task<PagedResult<TResult>> ProjectToPagedAsync<TResult>(PaginationFilter filter, ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification) ?? DbContext.Set<TEntity>();
            var totalItems = await query.CountAsync(cancellationToken);
            var pagination = new Pagination(totalItems, filter);

            var data = await query
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ProjectTo<TResult>(ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new PagedResult<TResult>(data, pagination);
        }

        public async Task<List<TResult>> ProjectToListAsync<TResult>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);

            return await query
                .ProjectTo<TResult>(ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<TResult> ProjectToFirstAsync<TResult>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(specification);

            return await query
                .ProjectTo<TResult>(ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
