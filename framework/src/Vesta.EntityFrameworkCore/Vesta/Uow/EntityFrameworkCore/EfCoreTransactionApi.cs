using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Uow.EntityFrameworkCore
{
    public class EfCoreTransactionApi : ITransactionApi, ISupportRollback
    {

        internal protected IDbContextTransaction DbContextTransaction { get; }

        public IEfCoreDbContext StarterDbContext { get; }

        public List<IEfCoreDbContext> AttendedDbContexts { get; }

        public EfCoreTransactionApi(
            IDbContextTransaction dbContextTransaction,
            IEfCoreDbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<IEfCoreDbContext>();
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

        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }

        internal static string GetKey(IEfCoreDbContext dbContext)
        {
            return $"EntityFrameworkCore_{dbContext.GetConnectionString()}";
        }
    }
}
