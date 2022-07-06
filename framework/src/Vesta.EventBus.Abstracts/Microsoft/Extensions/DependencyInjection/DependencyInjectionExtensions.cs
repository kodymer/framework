namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaEventBusAbstracts(this IServiceCollection services)
        {
            services.AddVestaCore();
        }
    }
}
