using Ardalis.Specification;
using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Ddd.Domain.Repositories
{
    public interface IRepository
    {

    }

    public interface IRepository<T> : IRepositoryBase<T>, IReadOnlyRepository<T> 
        where T : class
    {
        Task<T> AddAsync(T entity, bool save, CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool save, CancellationToken cancellationToken = default);

        Task<int> UpdateAsync(T entity, bool save, CancellationToken cancellationToken = default);

        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool save, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(T entity, bool save, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync<TKey>(TKey id, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync<TKey>(TKey id, bool save, CancellationToken cancellationToken = default);

        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool save, CancellationToken cancellationToken = default);

        Task<int> DeleteRangeAsync(ISpecification<T> specification, bool save, CancellationToken cancellationToken = default);
    }
}
