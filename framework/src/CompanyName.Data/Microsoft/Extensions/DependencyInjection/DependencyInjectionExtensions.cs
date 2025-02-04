namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameData(this IServiceCollection services)
        {
            services.AddCompanyNameCore();
        }
    }
}
