using Samples.Banks.Accounts;

namespace Samples.Banks.Transfers
{
    public interface IBankTransferService
    {
        Task<BankTransfer> MakeTransferAsync(BankAccount accountFrom, BankAccount accountTo, decimal amount);
    }
}