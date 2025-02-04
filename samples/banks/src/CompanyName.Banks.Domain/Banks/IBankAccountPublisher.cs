namespace CompanyName.Banks
{
    public interface IBankAccountPublisher
    {
        Task PublishAsync(BankAccount account, CancellationToken cancellationToken = default);
    }
}