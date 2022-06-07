using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.Uow.EntityFrameworkCore
{
    public abstract class EfCoreDbContextProvider<TDbContext>
        where TDbContext : class, IEfCoreDbContext
    {
        protected IUnitOfWork UnitOfWork { get; }

        public EfCoreDbContextProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract Task<TDbContext> GetDbContextAsync();
    }
}
