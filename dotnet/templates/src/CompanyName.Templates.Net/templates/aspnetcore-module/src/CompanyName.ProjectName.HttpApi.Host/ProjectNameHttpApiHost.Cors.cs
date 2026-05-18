namespace CompanyName.ProjectName
{
    public static class CorsProjectNameHttpApiHostHost
    {

        internal static IServiceCollection AddProjectNameCors(this IServiceCollection services, IConfiguration configuration)
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
