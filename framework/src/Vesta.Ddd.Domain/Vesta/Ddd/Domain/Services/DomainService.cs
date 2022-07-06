using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.Core.DependencyInjection;
using Vesta.Security.Users;

namespace Vesta.Ddd.Domain.Services
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
