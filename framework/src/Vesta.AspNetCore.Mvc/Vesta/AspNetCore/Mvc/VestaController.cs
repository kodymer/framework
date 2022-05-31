using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.Core.DependencyInjection;

namespace Vesta.AspNetCore.Mvc
{
    public class VestaController : Controller, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected ILogger Logger => loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILoggerFactory loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
