using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace CompanyName.Localization
{
    public class CompanyNameResourceManagerStringLocalizerFactory : IStringLocalizerFactory
    {

        public ILogger<CompanyNameResourceManagerStringLocalizerFactory> Logger { get; set; }

        private CompanyNameLocalizationOptions _options;
        private readonly ResourceManagerStringLocalizerFactory _resourceManagerStringLocalizerFactory;

        /// <summary>
        /// Creates a new <see cref="ResourceManagerStringLocalizer"/>.
        /// </summary>
        /// <param name="localizationOptions">The <see cref="IOptions{CompanyNameLocalizationOptions}"/>.</param>
        /// <param name="resourceManagerStringLocalizerFactory">The <see cref="ResourceManagerStringLocalizerFactory"/>.</param>
        public CompanyNameResourceManagerStringLocalizerFactory(
            IOptions<CompanyNameLocalizationOptions> localizationOptions,
            ResourceManagerStringLocalizerFactory resourceManagerStringLocalizerFactory)
        {
            Guard.IsNotNull(localizationOptions);
            Guard.IsNotNull(resourceManagerStringLocalizerFactory);

            _options = localizationOptions.Value;
            _resourceManagerStringLocalizerFactory = resourceManagerStringLocalizerFactory;

            Logger = NullLogger<CompanyNameResourceManagerStringLocalizerFactory>.Instance;

        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource is not null)
            {
                return _resourceManagerStringLocalizerFactory?.Create(resourceSource);
            }

            var stringLocalizer = _resourceManagerStringLocalizerFactory?.Create(_options.DefaultResourceType);
            if (stringLocalizer is null)
            {
                throw new CompanyNameException($"Set {nameof(resourceSource)} or define the default localization resource type (by configuring the {nameof(CompanyNameLocalizationOptions)}.{nameof(CompanyNameLocalizationOptions.DefaultResourceType)}) to be able to use the L object!");
            }
            else
            {
                return stringLocalizer;
            }
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return _resourceManagerStringLocalizerFactory.Create(baseName, location);
        }
    }
}
