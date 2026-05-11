using Ardalis.Specification;
using CompanyName.Ddd.Domain.Common.Filters;
using CompanyName.Ddd.Domain.Common.Pagination;

namespace CompanyName.Ddd.Domain.Repositories
{
    public interface IReadOnlyRepository<T> : IReadRepositoryBase<T>, IRepository
        where T : class
    {

        Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);

        Task<PagedResult<TResult>> ProjectToPagedAsync<TResult>(
            PaginationFilter filter, ISpecification<T> specification = null, CancellationToken cancellationToken = default);

        Task<List<TResult>> ProjectToListAsync<TResult>(
            ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<TResult?> ProjectToFirstAsync<TResult>(
            ISpecification<T> specification, CancellationToken cancellationToken = default);
    }
}
