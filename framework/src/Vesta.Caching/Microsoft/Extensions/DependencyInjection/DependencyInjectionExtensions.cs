namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaCaching(this IServiceCollection services)
        {
            services.AddVestaCore();

            services.AddDistributedMemoryCache(); 
        }
    }
}
