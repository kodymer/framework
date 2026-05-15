using Ardalis.Specification;

namespace CompanyName.Ddd.Domain.Repositories
{
    public interface IReadOnlyRepository<T> : IReadRepositoryBase<T>, IRepository
        where T : class
    {
        Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
    }
}
