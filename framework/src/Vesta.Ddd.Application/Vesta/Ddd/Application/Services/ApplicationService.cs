﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Vesta.Core.DependencyInjection;
using Vesta.Uow;

namespace Vesta.Ddd.Application.Services
{
    public abstract class ApplicationService : IApplicationService, IServiceProviderAccessor
    {

        public IServiceProvider ServiceProvider { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => ServiceProvider.GetRequiredService<IUnitOfWork>();

        protected IMapper ObjectMapper => ServiceProvider.GetService<IMapper>();

        protected ILogger Logger => _logger ??= _loggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance;

        private ILogger _logger;
        private ILoggerFactory _loggerFactory => ServiceProvider.GetService<ILoggerFactory>();

    }
}