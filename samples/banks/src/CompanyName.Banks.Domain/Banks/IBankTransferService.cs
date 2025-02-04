namespace CompanyName.Banks
{
    public interface IBankTransferService
    {
        Task<BankTransfer> MakeTransferAsync(BankAccount accountFrom, BankAccount accountTo, decimal amount);
    }
}