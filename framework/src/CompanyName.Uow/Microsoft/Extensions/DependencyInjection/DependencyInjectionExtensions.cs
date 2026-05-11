using CompanyName.Uow;
using LazyProxy.ServiceProvider;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameUow(this IServiceCollection services)
        {

            services
                .AddCompanyNameAutofac();

            services
                .AddOptions<UnitOfWorkDefaultOptions>();

            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>()
                .AddScoped<UnitOfWorkInterceptor>()
                .AddLazyScoped<IEventDispatcher, UnitOfWorkEventDispatcher>()
                .AddScoped<IEventStore, InMemoryEventStore>();

            return services;
        }
    }
}
