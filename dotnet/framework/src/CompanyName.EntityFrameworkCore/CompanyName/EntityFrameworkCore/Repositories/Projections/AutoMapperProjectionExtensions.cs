using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CompanyName.Ddd.Domain.Common.Filters;
using CompanyName.Ddd.Domain.Common.Pagination;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.Ddd.Domain.Repositories;
using CompanyName.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.EntityFrameworkCore.Repositories.Projections
{
    public static class AutoMapperProjectionExtensions
    {

        private static IRepositoryInternals<TEntity> GetRepositoryInternals<TEntity>(
            IReadOnlyRepository<TEntity> repository)
            where TEntity : class, IEntity
        {
            return (IRepositoryInternals<TEntity>)repository;
        }

        public static async Task<PagedResult<TResult>> ProjectPagedAsync<TEntity, TResult>(this IReadOnlyRepository<TEntity> repository, PaginationFilter filter, ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity
        {
            var internals = GetRepositoryInternals(repository);

            var query = internals.ApplySpecificationInternal(specification) ?? internals.CreateDbSet();

            var totalItems = await query.CountAsync(cancellationToken);
            var pagination = new Pagination(totalItems, filter);

            var mapperConfig = internals.MapperConfiguration as IConfigurationProvider;

            var data = await query
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ProjectTo<TResult>(mapperConfig)
                .ToListAsync(cancellationToken);

            return new PagedResult<TResult>(data, pagination);
        }

        public static async Task<List<TResult>> ProjectListAsync<TEntity, TResult>(this IReadOnlyRepository<TEntity> repository, ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity
        {
            var internals = GetRepositoryInternals(repository);

            var query = internals.ApplySpecificationInternal(specification);

            var mapperConfig = internals.MapperConfiguration as IConfigurationProvider;

            return await query
                .ProjectTo<TResult>(mapperConfig)
                .ToListAsync(cancellationToken);
        }

        public static async Task<TResult> ProjectFirstAsync<TResult, TEntity>(this IReadOnlyRepository<TEntity> repository, ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity
        {
            var internals = GetRepositoryInternals(repository);

            var query = internals.ApplySpecificationInternal(specification);

            var mapperConfig = internals.MapperConfiguration as IConfigurationProvider;

            return await query
                .ProjectTo<TResult>(mapperConfig)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
