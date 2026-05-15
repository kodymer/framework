using CommunityToolkit.Diagnostics;
using CompanyName.EntityFrameworkCore.Abstractions;

namespace CompanyName.Uow.EntityFrameworkCore
{
    public class EfCoreDatabaseApi : IDatabaseApi, ISupportSavingChanges
    {
        public IExtendedDbContext DbContext { get; }

        public EfCoreDatabaseApi(IExtendedDbContext dbContext)
        {
            Guard.IsNotNull(dbContext, nameof(dbContext));

            DbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        internal static string GetKey(IExtendedDbContext dbContext)
        {
            return $"{dbContext.GetType().FullName}_{dbContext.GetConnectionString()}";
        }

    }
}
