using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Vesta.ProjectName.Options;

namespace Vesta.ProjectName.Configuration
{
    public static class SwaggerConfiguration
    {

        public static void AddSwagger(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionName)
                .Get<AuthenticationOptions>();

            //Servicio de Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "[Asisa][Vesta][ProjectName] API", Version = "v1" });
                options.DescribeAllParametersInCamelCase();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "oauth2",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authenticationOptions.Authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authenticationOptions.Authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { authenticationOptions.Scope, "Base net6 API" }
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IConfiguration configuration)
        {
            var applicationOptions = configuration
                .GetSection(ApplicationOptions.SectionName)
                .Get<ApplicationOptions>();

            var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionName)
                .Get<AuthenticationOptions>();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{applicationOptions.PathPrefix}" } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{applicationOptions.PathPrefix}/swagger/v1/swagger.json", "[Asisa][Vesta][ProjectName] API v1");
                c.OAuthClientId(authenticationOptions.SwaggerClientId);
                c.OAuthAppName("Base API");
                c.OAuthUsePkce();
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            return app;
        }
    }
}
