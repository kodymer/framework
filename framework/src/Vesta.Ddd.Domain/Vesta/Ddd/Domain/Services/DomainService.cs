using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.Core.DependencyInjection;

namespace Vesta.Ddd.Domain.Services
{
    public abstract class DomainService : IDomainService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected ILogger Logger => loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILoggerFactory loggerFactory => ServiceProvider.GetService<ILoggerFactory>();
    }
}
