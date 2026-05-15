using CompanyName.EntityFrameworkCore.Repositories;
using Samples.Banks.Traceability;

namespace Samples.Banks.EntityFrameworkCore.Repositories
{
    public class ErrorRepository : EfCoreRepository<TraceabilityDbContext, ErrorRecord>, IErrorRepository
    {
        public ErrorRepository(TraceabilityDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
