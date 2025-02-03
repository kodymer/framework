using Vesta.Ddd.Domain.Repositories;

namespace Vesta.Banks
{
    public interface IBankTransferRepository
    {

        Task InsertAsync(BankTransfer bankTransfer, CancellationToken cancellationToken = default);

        Task<IEnumerable<BankTransfer>> GelAllAsync(CancellationToken cancellationToken = default);
    }
}