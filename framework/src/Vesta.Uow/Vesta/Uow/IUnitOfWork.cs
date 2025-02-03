using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Core.DependencyInjection;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    public interface IUnitOfWork : IDisposable, IDatabaseApiContainer, ITransactionApiContainer, IServiceProviderAccessor
    {
        event EventHandler Completed;

        event EventHandler Completing;

        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        event EventHandler Disposed;

        event EventHandler Rollbacked;

        bool IsCompleting { get; }

        bool IsCompleted { get; }

        bool IsReversed { get; }

        IUnitOfWorkOptions Options { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task CompleteAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);

        internal Task AddEventRecordAsync<TPublisher>(UnitOfWorkEventRecord unitOfWorkEventRecord, long priority, CancellationToken cancellationToken = default)
            where TPublisher : class, IEventBus;

    }
}
