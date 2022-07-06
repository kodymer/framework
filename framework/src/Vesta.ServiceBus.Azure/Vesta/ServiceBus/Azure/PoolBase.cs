using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Concurrent;
using Vesta.Core.DependencyInjection;

namespace Vesta.ServiceBus.Azure
{
    public abstract class PoolBase<T> : IServiceProviderAccessor
        where T : class, IAsyncDisposable
    {
        public virtual IServiceProvider ServiceProvider { get; set; }

        protected  ConcurrentDictionary<string, Lazy<T>> Elements { get; }
        protected ILogger Logger => LoggerFactory.CreateLogger(GetType().Name);
        protected bool IsDisposed { get; set; }

        private ILoggerFactory LoggerFactory => ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

        public PoolBase()
        {
            Elements = new ConcurrentDictionary<string, Lazy<T>>();
        }

        protected virtual Lazy<T> GetOrAdd(string key, Lazy<T> element)
        {
            Logger.LogInformation($"Creating {typeof(T).Name} element ({key}).");

            return Elements.GetOrAdd(key, element);
        }

        public async virtual ValueTask DisposeAsync()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!Elements.Any())
            {
                Logger.LogDebug($"Disposed {typeof(T).Name} pool without elements in the pool");
                return;
            }

            Logger.LogInformation($"Disposing {typeof(T).Name} pool ({Elements.Count} elements).");

            foreach (var element in Elements.Values)
            {
                await DisposeAsync(element.Value);
            }

            Elements.Clear();

        }

        protected async virtual Task DisposeAsync(T element)
        {
            await element.DisposeAsync();
        }
    }
}

