namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddCompanyNameEventBusAbstracts(this IServiceCollection services)
        {
            services.AddCompanyNameCore();
        }
    }
}
