namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaData(this IServiceCollection services)
        {
            services.AddVestaCore();
        }
    }
}
