using Microsoft.EntityFrameworkCore;

namespace CompanyName.Data.Fixtures
{
    public class InMemoryCompanyNameDbContext : InMemoryCompanyNameDbContextBase<InMemoryCompanyNameDbContext>
    {
        public InMemoryCompanyNameDbContext(DbContextOptions<InMemoryCompanyNameDbContext> options)
            : base(options)
        {
        }
    }
}
