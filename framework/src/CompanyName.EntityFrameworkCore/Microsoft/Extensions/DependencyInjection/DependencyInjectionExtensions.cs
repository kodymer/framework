using LazyProxy.ServiceProvider;
using CompanyName.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddCompanyNameEntityFrameworkCoreAbstracts();
            services.AddCompanyNameAuditing();
            services.AddCompanyNameDddDomain();
            services.AddCompanyNameUow();
            services.AddCompanyNameData();

            services.AddScoped<IUnitOfWorkEventRecordRegistrar, UnitOfWorkEventRecordRegistrar>();
        }
    }
}
