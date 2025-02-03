using Vesta.Banks.Traceability;
using Vesta.Domain.EntityFrameworkCore.Repositories;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Banks.EntityFrameworkCore.Repositories
{
    public class ErrorRepository : EfCoreRepository<TraceabilityDbContext, Error, Guid>, IErrorRepository
    {
        public ErrorRepository(IDbContextProvider<TraceabilityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
