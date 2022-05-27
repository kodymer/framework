using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.ProjectName.Bank;
using Vesta.ProjectName.EntityFrameworkCore.Repositories;

namespace Vesta.ProjectName.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
        }
    }
}
