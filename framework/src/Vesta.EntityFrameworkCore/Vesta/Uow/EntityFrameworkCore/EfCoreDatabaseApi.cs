using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Uow.EntityFrameworkCore
{
    public class EfCoreDatabaseApi : IDatabaseApi, ISupportSavingChanges
    {
        public IEfCoreDbContext DbContext { get; }

        public EfCoreDatabaseApi(IEfCoreDbContext dbContext)
        {
            Guard.Against.Null(dbContext, nameof(dbContext));

            DbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        internal static string GetKey(IEfCoreDbContext dbContext)
        {
            return  $"{dbContext.GetType().FullName}_{dbContext.GetConnectionString()}";
        }

    }
}
