using System.Runtime.CompilerServices;

[assembly: 
    InternalsVisibleTo("Vesta.Ddd.Application")]

namespace Vesta.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Begin(UnitOfWorkOptions options);

        internal IUnitOfWork Create();
    }
}