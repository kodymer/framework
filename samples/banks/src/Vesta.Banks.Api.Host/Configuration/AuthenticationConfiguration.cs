using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vesta.Banks.Options;

namespace Vesta.Banks.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authenticationOptions = configuration
                    .GetSection(AuthenticationOptions.SectionName)
                    .Get<AuthenticationOptions>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authenticationOptions.Authority;
                    options.Audience = authenticationOptions.Audience;

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = authenticationOptions.Audience,

                        ValidateIssuer = true,
                        ValidIssuer = authenticationOptions.Authority

                    };
                    options.Validate();


                });

            return services;
        }
    }
}