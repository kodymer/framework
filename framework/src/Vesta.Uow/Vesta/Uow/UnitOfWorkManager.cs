using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWorkManager(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options)
        {
            var unitOfWork = (this as IUnitOfWorkManager).Create();
            unitOfWork.As<UnitOfWork>().Initialize(options);

            return unitOfWork;
        }

        IUnitOfWork IUnitOfWorkManager.Create()
        {
            IServiceScope scope = null;
            IUnitOfWork unitOfWork = null;

            try
            {
                scope = _serviceScopeFactory.CreateScope();

                var eventPublishingManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkEventPublishingManager>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>();
                unitOfWork = new UnitOfWork(scope.ServiceProvider, eventPublishingManager, options);

                unitOfWork.Disposed += (object sender, EventArgs e) =>
                {
                    scope.Dispose();
                };
            }
            catch (Exception)
            {
                scope.Dispose();

                throw;
            }

            return unitOfWork;
        }
    }
}


