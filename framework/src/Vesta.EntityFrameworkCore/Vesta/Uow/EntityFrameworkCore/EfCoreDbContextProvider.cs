using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core;
using Vesta.Core.DependencyInjection;
using Vesta.EntityFrameworkCore;

namespace Vesta.Uow.EntityFrameworkCore
{
    public abstract class EfCoreDbContextProvider<TDbContext> 
        where TDbContext : class, IEfCoreDbContext
    {
        protected  IUnitOfWork UnitOfWork { get; }

        public EfCoreDbContextProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract Task<TDbContext> GetDbContextAsync();
    }
}
