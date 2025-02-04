using CompanyName.Banks.Traceability;
using CompanyName.Domain.EntityFrameworkCore.Repositories;
using CompanyName.EntityFrameworkCore.Abstracts;

namespace CompanyName.Banks.EntityFrameworkCore.Repositories
{
    public class ErrorRepository : EfCoreRepository<TraceabilityDbContext, Error, Guid>, IErrorRepository
    {
        public ErrorRepository(IDbContextProvider<TraceabilityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
