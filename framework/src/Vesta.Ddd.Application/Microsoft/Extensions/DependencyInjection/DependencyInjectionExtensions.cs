namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaDddApplication(this IServiceCollection services)
        {
            services.AddVestaUow();
        }
    }
}
