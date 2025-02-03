using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CompanyName.Core.DependencyInjection;

namespace CompanyName.AspNetCore.Mvc
{
    public class CompanyNameController : Controller, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected ILogger Logger => loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILoggerFactory loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
