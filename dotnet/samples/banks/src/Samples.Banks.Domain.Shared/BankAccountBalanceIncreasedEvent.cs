namespace Samples.Banks
{
    public record class BankAccountBalanceIncreasedEvent(Guid Id, string Number, decimal AddedAmount, decimal Balance);
}
