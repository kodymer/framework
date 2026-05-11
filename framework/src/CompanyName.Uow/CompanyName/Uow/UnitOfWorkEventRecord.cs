using CommunityToolkit.Diagnostics;
using System;

namespace CompanyName.Uow
{
    public class UnitOfWorkEventRecord
    {
        public object Source { get; private set; }
        public object Data { get; private set; }

        public UnitOfWorkEventRecord(object source, object data)
        {
            Guard.IsNotNull(source);
            Guard.IsNotNull(data);

            Source = source;
            Data = data;
        }
    }
}
