using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.TestBase.Fixtures;

namespace Vesta.Banks.Fixtures
{
    public class ApplicationServiceRegistrarFixture : ServiceRegistrarFixture
    {

        public override void ConfigureServices(ServiceCollection services)
        {
            services.AddVestaAutoMapper(typeof(BanksApplicationStartup).Assembly);
        }
    }
}
