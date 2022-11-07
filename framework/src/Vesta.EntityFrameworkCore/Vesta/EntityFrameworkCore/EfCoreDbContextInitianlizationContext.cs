using Vesta.Uow;

namespace Vesta.EntityFrameworkCore
{
    public class EfCoreDbContextInitianlizationContext
    {
        public EfCoreDbContextInitianlizationContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }
    }
}