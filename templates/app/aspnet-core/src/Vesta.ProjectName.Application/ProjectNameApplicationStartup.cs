﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vesta.ProjectName.Configuration;
using Vesta.ProjectName.EntityFrameworkCore;

namespace Vesta.ProjectName
{
    public static class ProjectNameApplicationStartup
    {
        public static IServiceCollection AddProjectNameApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddProjectNameDomain(configuration)
                .AddProjectNameEntityFrameworkCore(configuration)
                .AddProjectNameAppSevices();

            services
                .AddVestaDddApplication();

            services
                .AddVestaCachingStackExchangeRedis();

            services
                .AddVestaAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }


    }
}
