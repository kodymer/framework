using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CompanyName.Core.DependencyInjection;
using CompanyName.Security.Users;
using Microsoft.Extensions.Localization;
using CompanyName.Localization.Resources;

namespace CompanyName.Ddd.Domain.Services
{
    public abstract class DomainService : IDomainService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }


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

        private ILogger _logger;

        private ILoggerFactory _loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
