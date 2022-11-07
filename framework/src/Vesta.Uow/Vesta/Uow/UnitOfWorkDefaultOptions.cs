using System.Data;

namespace Vesta.Uow
{
    public class UnitOfWorkDefaultOptions : IUnitOfWorkOptions
    {
        public bool IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public int? Timeout { get; set; }

        internal UnitOfWorkOptions Normalize(UnitOfWorkOptions options)
        {
            options.IsolationLevel ??= IsolationLevel;

            options.Timeout ??= Timeout;

            return options;
        }
    }
}
