﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Vesta.EntityFrameworkCore.SqlServer
{
    public abstract class VestaDbContext<TDbContext> : VestaDbContextBase<TDbContext>, ISupportConnection
        where TDbContext : DbContext
    {
        string ISupportConnection.ConnectionString { get; set; }

        public VestaDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
            var conn = Database.GetDbConnection().ConnectionString;

            (this as ISupportConnection).ConnectionString = GetConnectionString(options);
        }

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<pendiente>")]
        protected override string GetConnectionString(DbContextOptions<TDbContext> options)
        {
            var extension = options.Extensions.First(e => e is SqlServerOptionsExtension);
            var sqlServerOptionsExtension = (SqlServerOptionsExtension)extension;
            return sqlServerOptionsExtension.Connection?.ConnectionString ??
                sqlServerOptionsExtension.ConnectionString;
        }
    }
}
