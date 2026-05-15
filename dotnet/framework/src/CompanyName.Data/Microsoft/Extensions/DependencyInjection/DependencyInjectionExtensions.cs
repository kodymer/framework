namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameData(this IServiceCollection services)
        {
            services
                .AddCompanyNameCore();

            return services;
        }
    }
}
