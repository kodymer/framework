using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.EntityFrameworkCore
{
    public interface IStartableEfCoreDbContext : IEfCoreDbContext
    {
        internal void Initialize(EfCoreDbContextInitianlizationContext initializationContext);
    }
}
