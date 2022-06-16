using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Vesta.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperServiceCollectionExtensions
    {

        public static void AddVestaDatabase<TDatabase>(this IServiceCollection services, Action<DatabaseOptionsBuilder> optionsBuilder)
            where TDatabase : VestaDatabase<TDatabase>, new()
        {
            services.AddVestaDapper();

            services.Configure<DatabaseOptions>(options => {
                var builder = new DatabaseOptionsBuilder(options);
                optionsBuilder.Invoke(builder);
            });

            services.AddTransient<TDatabase>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                var database = VestaDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                return database;
            });
        }

        public static void AddVestaDatabase<TDatabase>(this IServiceCollection services)
            where TDatabase : VestaDatabase<TDatabase>, new()
        {
            services.AddVestaDapper();

            services.AddTransient<TDatabase>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsFactory<DatabaseOptions>>().Create(null);
                var database = VestaDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                return database;
            });
        }
    }
}
