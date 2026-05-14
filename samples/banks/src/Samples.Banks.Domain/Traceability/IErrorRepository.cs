using CompanyName.Ddd.Domain.Repositories;

namespace Samples.Banks.Traceability
{
    public interface IErrorRepository : IRepository<ErrorRecord>
    {
    }
}