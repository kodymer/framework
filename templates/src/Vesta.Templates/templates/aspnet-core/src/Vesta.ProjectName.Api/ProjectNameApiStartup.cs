﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Vesta.ProjectName
{
    public static class ProjectNameApiStartup
    {
        public static void AddProjectNameApi(this IServiceCollection services)
        {
            services
                .AddProjectNameApplication();

            services
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services
                .AddVestaAspNetCoreMvc(jsonConfigure: options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        }
    }
}
