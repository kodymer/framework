using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Banks.Bank;
using Vesta.Banks.EntityFrameworkCore.Repositories;

namespace Vesta.Banks.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddBanksRepositories(this IServiceCollection services)
        {
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
        }
    }
}
