namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameEventBusAbstracts(this IServiceCollection services)
        {
            services
                .AddCompanyNameCore();

            return services;
        }
    }
}
