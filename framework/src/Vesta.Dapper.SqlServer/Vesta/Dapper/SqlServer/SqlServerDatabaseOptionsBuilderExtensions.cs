namespace Vesta.Dapper.SqlServer
{
    public static class SqlServerDatabaseOptionsBuilderExtensions
    {
        public static DatabaseOptionsBuilder UseSqlServer(this DatabaseOptionsBuilder builder, string connectionString)
        {
            builder.Options.ConnectionString = connectionString;

            return builder;
        }
    }
}
