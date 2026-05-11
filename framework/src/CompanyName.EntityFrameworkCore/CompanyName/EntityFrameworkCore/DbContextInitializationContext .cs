using CompanyName.Uow;

namespace CompanyName.EntityFrameworkCore
{
    public class DbContextInitializationContext
    {
        public DbContextInitializationContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }
    }
}