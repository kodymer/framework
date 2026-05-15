using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyName.Caching
{
    public class HybridCacheBuilder(IServiceCollection services) : IHybridCacheBuilder
    {
        public IServiceCollection Services => services;
    }
}
