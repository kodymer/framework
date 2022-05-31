using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.AutoMapper;
using Vesta.Core.DependencyInjection;
using Vesta.Uow;

namespace Vesta.Ddd.Application.Services
{
    public abstract class ApplicationService : IApplicationService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => ServiceProvider.GetRequiredService<IUnitOfWork>();

        protected IMapper ObjectMapper => ServiceProvider.GetRequiredService<IMapper>();

        protected ILogger Logger => loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILoggerFactory loggerFactory => ServiceProvider.GetService<ILoggerFactory>();

    }
}