using CompanyName.EntityFrameworkCore.Abstracts;

namespace CompanyName.EntityFrameworkCore
{
    public interface IStartableEfCoreDbContext : IEfCoreDbContext
    {
        internal void Initialize(EfCoreDbContextInitianlizationContext initializationContext);
    }
}
