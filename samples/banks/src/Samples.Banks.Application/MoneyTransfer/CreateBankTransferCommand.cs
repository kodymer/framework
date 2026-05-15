namespace Samples.Banks.MoneyTransfers
{
    public record class CreateBankTransferCommand(
        Guid BankAccountFromId,
        Guid BankAccountToId,
        decimal Amount);

}
