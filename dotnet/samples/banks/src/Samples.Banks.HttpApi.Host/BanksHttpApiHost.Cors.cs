namespace Samples.Banks
{
    public static class CorsBanksHttpApiHostHost
    {

        internal static IServiceCollection AddBanksCors(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCors(o => o.AddPolicy("default", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
            return services;
        }
    }
}
