using CompanyName.Core.DependencyInjection;
using CompanyName.Localization.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CompanyName.AspNetCore.Mvc
{
    public class CompanyNameController : Controller, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected ILogger Logger => _logger ??= loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

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


        private IStringLocalizer _stringLocalizer;

        private Type _localizationResource = typeof(DefaultResource);

        private ILogger _logger;

        private ILoggerFactory loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
