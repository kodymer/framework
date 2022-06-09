using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ProjectName.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddProjectNameAutoMapper(this IServiceCollection services)
        {
            services.AddVestaAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
