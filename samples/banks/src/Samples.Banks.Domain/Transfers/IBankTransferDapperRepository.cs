namespace Samples.Banks.Transfers
{
    // TO-DO: rename to IBankTransferRepository and use  dependency Injection keyed feature
    public interface IBankTransferDapperRepository
    {

        Task AddAsync(BankTransfer bankTransfer, CancellationToken cancellationToken = default);

        Task<IEnumerable<BankTransfer>> ListAsync(CancellationToken cancellationToken = default);
    }
}