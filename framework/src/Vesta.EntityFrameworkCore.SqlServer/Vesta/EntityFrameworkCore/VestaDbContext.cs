using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Vesta.EntityFrameworkCore
{
    public abstract class VestaDbContext<TDbContext> : VestaDbContextBase<TDbContext>
        where TDbContext : DbContext
    {

        public VestaDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
            
        }

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<pendiente>")]
        public override string GetConnectionString()
        {
            var extension = Options.Extensions.First(e => e is SqlServerOptionsExtension);
            var sqlServerOptionsExtension = (SqlServerOptionsExtension)extension;
            return sqlServerOptionsExtension.Connection?.ConnectionString ??
                sqlServerOptionsExtension.ConnectionString;
        }
    }
}
