using CompanyName.Banks.Traceability;
using CompanyName.EntityFrameworkCore.Repositories;

namespace CompanyName.Banks.EntityFrameworkCore.Repositories
{
    public class ErrorRepository : EfCoreRepository<TraceabilityDbContext, Error>, IErrorRepository
    {
        public ErrorRepository(TraceabilityDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
