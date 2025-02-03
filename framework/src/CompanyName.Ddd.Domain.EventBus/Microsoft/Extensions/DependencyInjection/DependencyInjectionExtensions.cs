namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameDddDomainEventBus(this IServiceCollection services)
        {
            services.AddCompanyNameCore();
        }
    }
}
