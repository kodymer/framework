using Microsoft.Extensions.Options;
using CompanyName.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameDapper(this IServiceCollection services)
        {
            services.AddCompanyNameDddDomain();
            services.AddCompanyNameData();

            services.AddSingleton<IOptionsFactory<DatabaseOptions>, DatabaseOptionsFactory>();
        }
    }
}
