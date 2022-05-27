using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core.DependencyInjection;

namespace Vesta.Uow
{
    public interface IUnitOfWork : IDatabaseApiContainer, IServiceProviderAccessor
    {

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
