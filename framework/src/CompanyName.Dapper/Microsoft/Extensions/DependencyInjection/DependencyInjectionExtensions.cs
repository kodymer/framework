using CompanyName.Dapper;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameDapper(this IServiceCollection services)
        {
            services
                .AddCompanyNameDddDomain()
                .AddCompanyNameData()
                .AddSingleton<IOptionsFactory<DatabaseOptions>, DatabaseOptionsFactory>();

            return services;
        }
    }
}
