namespace Samples.Banks
{
    public record class BankAccountBalanceDecreasedEvent(Guid Id, string Number, decimal SubtractedAmount, decimal Balance);
}
