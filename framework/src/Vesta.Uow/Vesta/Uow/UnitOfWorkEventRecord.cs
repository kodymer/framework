using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Uow
{
    public class UnitOfWorkEventRecord
    {
        public object Source { get; private set; }
        public object Data { get; private set; }

        public UnitOfWorkEventRecord(object source, object data)
        {
            Guard.Against.Null(source, nameof(source));
            Guard.Against.Null(data, nameof(data));

            Source = source;
            Data = data;
        }
    }
}
