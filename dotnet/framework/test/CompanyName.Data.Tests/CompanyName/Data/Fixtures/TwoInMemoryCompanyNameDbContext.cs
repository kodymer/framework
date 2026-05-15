using Microsoft.EntityFrameworkCore;

namespace CompanyName.Data.Fixtures
{
    public class TwoInMemoryCompanyNameDbContext : InMemoryCompanyNameDbContextBase<TwoInMemoryCompanyNameDbContext>
    {
        public TwoInMemoryCompanyNameDbContext(DbContextOptions<TwoInMemoryCompanyNameDbContext> options)
            : base(options)
        {
        }
    }
}
