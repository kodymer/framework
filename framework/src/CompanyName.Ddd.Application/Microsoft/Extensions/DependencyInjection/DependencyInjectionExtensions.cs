namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCompanyNameDddApplication(this IServiceCollection services)
        {
            services
                .AddCompanyNameDddDomain()
                .AddCompanyNameAutoMapper()
                .AddCompanyNameLocalization()
                .AddCompanyNameSecurity()
                .AddCompanyNameUow();

            return services;
        }
    }
}
