using CompanyName.Ddd.Domain.Repositories;

namespace CompanyName.Banks
{
    public interface IBankTransferRepository
    {

        Task InsertAsync(BankTransfer bankTransfer, CancellationToken cancellationToken = default);

        Task<IEnumerable<BankTransfer>> GelAllAsync(CancellationToken cancellationToken = default);
    }
}