namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameCaching(this IServiceCollection services)
        {
            services.AddCompanyNameCore();

            services.AddDistributedMemoryCache(); 
        }
    }
}
