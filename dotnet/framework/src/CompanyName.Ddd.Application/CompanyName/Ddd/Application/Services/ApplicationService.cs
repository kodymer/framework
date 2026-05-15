using Autofac.Extras.DynamicProxy;
using AutoMapper;
using CompanyName.AutoMapper;
using CompanyName.Core.DependencyInjection;
using CompanyName.Localization.Resources;
using CompanyName.Security.Users;
using CompanyName.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CompanyName.Ddd.Application.Services
{

    [Intercept(typeof(UnitOfWorkInterceptor))]
    public abstract class ApplicationService : IApplicationService, IServiceProviderAccessor
    {

        protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        protected IUnitOfWork CurrentUnitOfWork => _currentUnitOfWork ??= UnitOfWorkManager.Create();

        protected IMapper ObjectMapper => ServiceProvider.GetService<IMapperAccessor>()?.Mapper;

        protected ICurrentUser CurrenUser => ServiceProvider.GetService<ICurrentUser>();

        protected ILogger Logger => _logger ??= _loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        protected Type LocalizationResource
        {
            get
            {
                return _localizationResource;
            }
            set
            {
                _localizationResource = value;
                _stringLocalizer = null;
            }
        }

        protected IStringLocalizerFactory StringLocalizerFactory => ServiceProvider.GetService<IStringLocalizerFactory>();

        protected IStringLocalizer L => _stringLocalizer ??= StringLocalizerFactory?.Create(LocalizationResource);


        private Type _localizationResource = typeof(DefaultResource);

        private IStringLocalizer _stringLocalizer;

        private IUnitOfWork _currentUnitOfWork;

        private ILogger _logger;

        private ILoggerFactory _loggerFactory => ServiceProvider.GetService<ILoggerFactory>();


        public IServiceProvider ServiceProvider { get; set; }

    }
}