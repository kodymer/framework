using CompanyName.Ddd.Domain.Services;
using Samples.Banks.Accounts;


namespace Samples.Banks.Transfers
{
    public class BankTransferService : DomainService, IBankTransferService
    {

        public BankTransferService()
        {
        }

        public Task<BankTransfer> MakeTransferAsync(BankAccount accountFrom, BankAccount accountTo, decimal amount)
        {

            accountFrom.Decrease(amount);
            accountTo.Increase(amount);

            var bankTransfer = new BankTransfer(accountFrom.Number, accountTo.Number, amount);
            return Task.FromResult(bankTransfer);
        }
    }
}
