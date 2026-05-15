using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Uow
{
    public interface ITransactionApi : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken);

    }
}
