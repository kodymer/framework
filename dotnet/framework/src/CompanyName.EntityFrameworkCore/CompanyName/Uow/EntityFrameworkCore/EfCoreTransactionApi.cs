using CompanyName.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CompanyName.Uow.EntityFrameworkCore
{
    public class EfCoreTransactionApi : ITransactionApi, ISupportRollback
    {
        private bool _disposed;

        internal protected IDbContextTransaction DbContextTransaction { get; }

        public IExtendedDbContext StarterDbContext { get; }

        public List<IExtendedDbContext> AttendedDbContexts { get; }

        public EfCoreTransactionApi(
            IDbContextTransaction dbContextTransaction,
            IExtendedDbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<IExtendedDbContext>();
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager() &&
                    dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
                {
                    continue;
                }

                await dbContext.Database.CommitTransactionAsync(cancellationToken);
            }

            await DbContextTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager() &&
                    dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
                {
                    continue;
                }

                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
            }

            await DbContextTransaction.RollbackAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DbContextTransaction?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        ~EfCoreTransactionApi()
        {
            Dispose(disposing: false);
        }

        internal static string GetKey(IExtendedDbContext dbContext)
        {
            return $"EntityFrameworkCore_{dbContext.GetConnectionString()}";
        }
    }
}
