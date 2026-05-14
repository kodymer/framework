namespace Samples.Banks
{
    public record class BankAccountCreatedEvent(Guid Id, string Number, decimal InitialBalance);
}
