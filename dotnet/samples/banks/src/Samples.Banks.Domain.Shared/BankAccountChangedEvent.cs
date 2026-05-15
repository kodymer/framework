namespace Samples.Banks
{
    public record class BankAccountChangedEvent(Guid Id, string Number, decimal Balance);

}
