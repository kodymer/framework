namespace Samples.Banks.Accounts
{
    public interface IBankAccountPublisher
    {
        Task PublishAsync(BankAccount account, CancellationToken cancellationToken = default);
    }
}