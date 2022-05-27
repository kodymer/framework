using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Vesta.EntityFrameworkCore.SqlServer
{
    public abstract class VestaDbContext<TDbContext> : VestaDbContextBase<TDbContext>, IEfCoreDbContext, ISupportConnection
        where TDbContext : DbContext
    {
        string ISupportConnection.ConnectionString { get; set; }

        public VestaDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
            (this as ISupportConnection).ConnectionString = GetConnectionString(options);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<pendiente>")]
        private string GetConnectionString(DbContextOptions<TDbContext> options)
        {
            var extension = options.Extensions.First(e => e is SqlServerOptionsExtension);
            var sqlServerOptionsExtension = (SqlServerOptionsExtension)extension;
            return sqlServerOptionsExtension.Connection?.ConnectionString ?? 
                sqlServerOptionsExtension.ConnectionString;
        }
    }
}
