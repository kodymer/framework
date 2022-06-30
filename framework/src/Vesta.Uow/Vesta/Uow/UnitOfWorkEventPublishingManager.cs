using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.EventBus.Abstracts;

namespace Vesta.Uow
{
    internal class UnitOfWorkEventPublishingManager : IUnitOfWorkEventPublishingManager
    {
        private readonly IUnitOfWorkEventPublishingStore _store;

        public UnitOfWorkEventPublishingManager(
            IUnitOfWorkEventPublishingStore store)
        {
            _store = store;
        }

        public async Task CreateAsync(UnitOfWorkEventPublishing publishing, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(publishing, nameof(publishing));

            await _store.PushAsync(publishing);

        }

        public async Task PublishAllAsync(CancellationToken cancellationToken = default)
        {
            var allPublishing = _store.Get();
            if (allPublishing.Any())
            {
                for (int i = 0; i < allPublishing.Count(); i++)
                {
                    var publishing = await _store.PopAsync();
                    await publishing.SendAsync();
                }
            }
        }
    }
}
