namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameDddDomainEventBus(this IServiceCollection services)
        {
            services
                .AddCompanyNameCore();

            return services;
        }
    }
}
