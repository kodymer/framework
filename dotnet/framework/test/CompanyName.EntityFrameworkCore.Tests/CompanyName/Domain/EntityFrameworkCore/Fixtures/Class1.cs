using CompanyName.Data.Fixtures;
using CompanyName.TestBase.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Domain.EntityFrameworkCore.Fixtures
{
    public class EntityFrameworkCoreServiceRegistrarFixture : ServiceRegistrarFixture
    {
        public override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());

            services.AddDbContext<InMemoryCompanyNameDbContext>();
        }
    }
}


