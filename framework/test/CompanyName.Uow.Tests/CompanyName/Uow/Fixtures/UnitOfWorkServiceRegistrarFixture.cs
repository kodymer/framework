using CompanyName.TestBase.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Uow.Fixtures
{
    public class UnitOfWorkServiceRegistrarFixture : ServiceRegistrarFixture
    {
        public override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());

            services.AddCompanyNameEventBusAzure();
        }
    }
}
