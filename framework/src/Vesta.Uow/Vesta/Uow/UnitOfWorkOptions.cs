using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;

namespace Vesta.Uow
{

    public class UnitOfWorkOptions : IUnitOfWorkOptions
    {
        public bool IsTransactional { get;  set; }

        public IsolationLevel? IsolationLevel { get;  set; }

        public int? Timeout { get;  set; }

        public UnitOfWorkOptions()
        {

        }

        public UnitOfWorkOptions(bool isTransactional)
            : this()
        {
            IsTransactional = isTransactional;
        }

        public UnitOfWorkOptions(bool isTransactional, IsolationLevel isolationLevel)
            : this(isTransactional)
        {
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkOptions(bool isTransactional, IsolationLevel isolationLevel, int timeout)
            : this(isTransactional, isolationLevel)
        {
            Timeout = timeout;
        }
    }
}
