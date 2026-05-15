using CompanyName.EntityFrameworkCore.Abstractions;

namespace CompanyName.EntityFrameworkCore
{
    public interface IInitializableDbContext : IExtendedDbContext
    {
        internal void Initialize(DbContextInitializationContext initializationContext);
    }
}
