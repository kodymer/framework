using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Options;

namespace Vesta.ProjectName.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var authenticationOptions = configuration
                        .GetSection(AuthenticationOptions.SectionName)
                        .Get<AuthenticationOptions>();

                    options.Authority = authenticationOptions.Authority;
                    options.Audience = authenticationOptions.Audience;
                    
                });

            return services;
        }
    }
}