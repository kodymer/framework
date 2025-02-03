using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using CompanyName.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperServiceCollectionExtensions
    {

        public static void AddCompanyNameDatabase<TDatabase>(this IServiceCollection services, Action<DatabaseOptionsBuilder> optionsBuilder)
            where TDatabase : CompanyNameDatabase<TDatabase>, new()
        {
            services.AddCompanyNameDapper();

            services.Configure<DatabaseOptions>(options => {
                var builder = new DatabaseOptionsBuilder(options);
                optionsBuilder.Invoke(builder);
            });

            services.AddTransient<TDatabase>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                var database = CompanyNameDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                return database;
            });
        }

        public static void AddCompanyNameDatabase<TDatabase>(this IServiceCollection services)
            where TDatabase : CompanyNameDatabase<TDatabase>, new()
        {
            services.AddCompanyNameDapper();

            services.AddTransient<TDatabase>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptionsFactory<DatabaseOptions>>().Create(null);
                var database = CompanyNameDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                return database;
            });
        }
    }
}
