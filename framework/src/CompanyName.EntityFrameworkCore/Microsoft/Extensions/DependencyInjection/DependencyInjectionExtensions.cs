using LazyProxy.ServiceProvider;
using CompanyName.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameEntityFrameworkCore(this IServiceCollection services)
        {
            services
                .AddCompanyNameDddApplication()
                .AddCompanyNameAuditing()
                .AddCompanyNameUow()
                .AddCompanyNameData();

            services
                .AddScoped<IUnitOfWorkEventRecordRegistrar, UnitOfWorkEventRecordRegistrar>();

            return services;
        }
    }
}
