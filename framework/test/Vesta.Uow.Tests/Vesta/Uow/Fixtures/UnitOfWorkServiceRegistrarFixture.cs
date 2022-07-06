using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.TestBase.Fixtures;

namespace Vesta.Uow.Fixtures
{
    public class UnitOfWorkServiceRegistrarFixture : ServiceRegistrarFixture
    {
        public override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            services.AddVestaEventBusAzure();
        }
    }
}
