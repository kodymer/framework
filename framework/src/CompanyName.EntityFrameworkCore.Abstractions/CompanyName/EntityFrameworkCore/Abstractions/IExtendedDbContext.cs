using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace CompanyName.EntityFrameworkCore.Abstractions
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public interface IExtendedDbContext : IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable
    {
        DatabaseFacade Database { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        string GetConnectionString();
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}