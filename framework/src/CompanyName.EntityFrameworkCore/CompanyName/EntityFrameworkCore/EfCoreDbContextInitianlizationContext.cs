using CompanyName.Uow;

namespace CompanyName.EntityFrameworkCore
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