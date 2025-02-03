namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameDddApplication(this IServiceCollection services)
        {
            services.AddCompanyNameSecurity();
            services.AddCompanyNameUow();
        }
    }
}
