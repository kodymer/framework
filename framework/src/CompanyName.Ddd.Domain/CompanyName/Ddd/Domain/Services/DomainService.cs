using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CompanyName.Core.DependencyInjection;
using CompanyName.Security.Users;

namespace CompanyName.Ddd.Domain.Services
{
    public abstract class DomainService : IDomainService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected ICurrentUser CurrenUser => ServiceProvider.GetService<ICurrentUser>();

        protected ILogger Logger => _logger ??= _loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILogger _logger;
        private ILoggerFactory _loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
