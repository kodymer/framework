using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
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

        protected ILogger Logger => ServiceProvider.GetRequiredService<IMapper>();

    }
}