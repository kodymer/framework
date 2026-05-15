using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.TestBase.Fixtures;
using Samples.Banks.Application;

namespace Samples.Banks.Fixtures
{
    public class ApplicationServiceRegistrarFixture : ServiceRegistrarFixture
    {

        public override void ConfigureServices(ServiceCollection services)
        {
            services.AddCompanyNameAutoMapper(typeof(BanksApplication).Assembly);
        }
    }
}
