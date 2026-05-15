namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameDddDomain(this IServiceCollection services)
        {
            services
                .AddCompanyNameAuditingAbstractions()
                .AddCompanyNameDddDomainEventBus()
                .AddCompanyNameSecurity();

            return services;
        }
    }
}
