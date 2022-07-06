using Microsoft.Extensions.Options;
using Vesta.Dapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaDapper(this IServiceCollection services)
        {
            services.AddVestaDddDomain();
            services.AddVestaData();

            services.AddSingleton<IOptionsFactory<DatabaseOptions>, DatabaseOptionsFactory>();
        }
    }
}
