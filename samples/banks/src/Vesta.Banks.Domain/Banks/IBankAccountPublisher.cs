namespace Vesta.Banks
{
    public interface IBankAccountPublisher
    {
        Task PublishAsync(BankAccount account, CancellationToken cancellationToken = default);
    }
}