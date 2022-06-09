using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Banks.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddBanksAutoMapper(this IServiceCollection services)
        {
            services.AddVestaAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
