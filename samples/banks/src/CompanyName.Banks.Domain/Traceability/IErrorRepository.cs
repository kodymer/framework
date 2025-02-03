using Vesta.Ddd.Domain.Repositories;

namespace Vesta.Banks.Traceability
{
    public interface IErrorRepository : IRepository<Error, Guid>
    {
    }
}