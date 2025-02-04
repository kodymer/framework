using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CompanyName.AutoMapper;
using CompanyName.Core.DependencyInjection;
using CompanyName.Security.Users;
using CompanyName.Uow;

namespace CompanyName.Ddd.Application.Services
{

    [Intercept(typeof(UnitOfWorkInterceptor))]
    public abstract class ApplicationService : IApplicationService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        protected IUnitOfWork CurrentUnitOfWork => _currentUnitOfWork ??= UnitOfWorkManager.Create();

        protected IMapper ObjectMapper => ServiceProvider.GetService<IMapperAccessor>()?.Mapper;

        protected ICurrentUser CurrenUser =>  ServiceProvider.GetService<ICurrentUser>();

        protected ILogger Logger => _logger ??= _loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private IUnitOfWork _currentUnitOfWork;

        private ILogger _logger;
        private ILoggerFactory _loggerFactory => ServiceProvider.GetService<ILoggerFactory>();

    }
}