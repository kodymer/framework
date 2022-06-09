namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVestaDddDomainEventBus(this IServiceCollection services)
        {
            services.AddVestaCore();
        }
    }
}
