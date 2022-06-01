using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Services;

namespace Vesta.Banks.Bank
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

            var bankTransfer = new BankTransfer(accountFrom.Id, accountTo.Id, amount);
            accountFrom.Debits.Add(bankTransfer);
            accountTo.Credits.Add(bankTransfer);

            return Task.FromResult(bankTransfer);
        }
    }
}
