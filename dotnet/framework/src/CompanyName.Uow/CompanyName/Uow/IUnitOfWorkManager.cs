using System.Runtime.CompilerServices;

[assembly: 
    InternalsVisibleTo("CompanyName.Ddd.Application")]

namespace CompanyName.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Begin(UnitOfWorkOptions options);

        internal IUnitOfWork Create();
    }
}