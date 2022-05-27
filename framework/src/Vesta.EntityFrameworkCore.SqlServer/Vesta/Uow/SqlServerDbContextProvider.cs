using Microsoft.Extensions.DependencyInjection;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.SqlServer;

namespace Vesta.Uow.EntityFrameworkCore.SqlServer
{
    public class SqlServerDbContextProvider<TDbContext> : EfCoreDbContextProvider<TDbContext>, IDbContextProvider<TDbContext>
         where TDbContext : class, IEfCoreDbContext
    {
        public SqlServerDbContextProvider(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public override Task<TDbContext> GetDbContextAsync()
        {
            var dbContextTypeFullName = typeof(TDbContext).FullName;
            var dbContext = UnitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            var connectionString = (dbContext as ISupportConnection).ConnectionString;
            var dbContextKey = $"{dbContextTypeFullName}_{connectionString}";

            var databaseApi = UnitOfWork.FindDatabaseApi(dbContextKey);
            if (databaseApi is null)
            {
                databaseApi = new EfCoreDatabaseApi(dbContext);
                UnitOfWork.AddDatabaseApi(dbContextKey, databaseApi);
            }

            return Task.FromResult((TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext);
        }
    }
}
