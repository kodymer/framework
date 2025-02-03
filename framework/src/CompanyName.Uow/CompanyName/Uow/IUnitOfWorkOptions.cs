using System.Data;

namespace CompanyName.Uow
{
    public interface IUnitOfWorkOptions
    {
        bool IsTransactional { get; internal set; }

        IsolationLevel? IsolationLevel { get; internal set; }

        int? Timeout { get; internal set; }
    }
}