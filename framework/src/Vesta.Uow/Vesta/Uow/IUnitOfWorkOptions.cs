using System.Data;

namespace Vesta.Uow
{
    public interface IUnitOfWorkOptions
    {
        bool IsTransactional { get; internal set; }

        IsolationLevel? IsolationLevel { get; internal set; }

        int? Timeout { get; internal set; }
    }
}