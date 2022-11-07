using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Uow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        public bool? IsTransactional { get; }

        public IsolationLevel? IsolationLevel { get; }

        public int? Timeout { get; }

        public UnitOfWorkAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel)
            : this(isTransactional)
        {
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel, int timeout)
    :       this(isTransactional, isolationLevel)
        {
            Timeout = timeout;
        }

        public void SetOptions(UnitOfWorkOptions options)
        {
            if (IsTransactional.HasValue)
            {
                options.IsTransactional = IsTransactional.Value;
            }

            if (Timeout.HasValue)
            {
                options.Timeout = Timeout.Value;
            }

            if (IsolationLevel.HasValue)
            {
                options.IsolationLevel = IsolationLevel.Value;
            }
        }
    }
}
