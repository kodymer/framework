using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core.DependencyInjection;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public interface IUnitOfWork : IDisposable, IDatabaseApiContainer, IServiceProviderAccessor
    {
        event EventHandler Completed;

        event EventHandler Completing;

        event EventHandler Failed;

        event EventHandler Disposing;

        bool IsCompleting { get; }

        bool IsCompleted { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task CompleteAsync(CancellationToken cancellationToken = default);

        internal Task AddEventRecordAsync<TPublisher>(UnitOfWorkEventRecord unitOfWorkEventRecord, long priority, CancellationToken cancellationToken = default)
            where TPublisher : class, IEventBus;
    }
}
