namespace Samples.Banks.Accounts
{
    public interface IBankAccountManager
    {
        Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default);
    }
}