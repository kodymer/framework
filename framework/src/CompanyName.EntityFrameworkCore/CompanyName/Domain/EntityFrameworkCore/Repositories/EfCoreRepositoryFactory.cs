using Ardalis.Specification.EntityFrameworkCore;
using CompanyName.AutoMapper;
using CompanyName.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstractions;
using Nito.AsyncEx;

namespace CompanyName.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryFactory<TRepository, TConcreteRepository, TContext> : IRepositoryFactory<TRepository>
        where TConcreteRepository : TRepository
        where TContext : CompanyNameDbContextBase<TContext>
    {
        private readonly IDbContextProvider<TContext> _contextProvider;

        public EfCoreRepositoryFactory(IDbContextProvider<TContext> contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public TRepository CreateRepository()
        {
            var dbContext = AsyncContext.Run(async () => await _contextProvider.GetDbContextAsync());

            var args = new object[] { dbContext };
            return (TRepository)Activator.CreateInstance(typeof(TConcreteRepository), args)!;
        }
    }
}
