using CompanyName.Dapper;
using CompanyName.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DapperServiceCollectionExtensions
    {

        public static IServiceCollection AddCompanyNameDatabase<TDatabase>(this IServiceCollection services, Action<DatabaseOptionsBuilder> optionsBuilder)
            where TDatabase : CompanyNameDatabase<TDatabase>, new()
        {
            services
                .AddCompanyNameDapper()
                .Configure<DatabaseOptions>(options => {
                    var builder = new DatabaseOptionsBuilder(options);
                    optionsBuilder.Invoke(builder);
                })
                .AddTransient<TDatabase>(serviceProvider => {
                    var options = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                    var database = CompanyNameDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                    return database;
                });

            return services;
        }

        public static IServiceCollection AddCompanyNameDatabase<TDatabase>(this IServiceCollection services, string connectionStringName = ConnectionStrings.DefaultNameConfig)
            where TDatabase : CompanyNameDatabase<TDatabase>, new()
        {
            services
                .AddCompanyNameDapper()
                .AddTransient<TDatabase>(serviceProvider =>
                {
                    var factory = serviceProvider.GetRequiredService<IOptionsFactory<DatabaseOptions>>().As<DatabaseOptionsFactory>();
                    factory.SetConnectionStringName(connectionStringName);

                    var options = factory.Create(null);
                    var database = CompanyNameDatabase<TDatabase>.Init(new SqlConnection(options.ConnectionString), options.CommandTimeout);
                    return database;
                });

            return services;
        }
    }
}
