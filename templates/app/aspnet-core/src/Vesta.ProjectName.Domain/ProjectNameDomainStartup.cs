using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.ProjectName.Bank;
using Vesta.ProjectName.Configuration;

namespace Vesta.ProjectName
{
    public static class ProjectNameDomainStartup
    {
        public static void AddProjectNameDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBankTransferService, BankTransferService>();
            
            services.AddDomainSevices();
        }
    }
}
