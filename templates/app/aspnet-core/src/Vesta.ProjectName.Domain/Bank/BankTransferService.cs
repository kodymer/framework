using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ProjectName.Bank
{
    public class BankTransferService : IBankTransferService
    {

        public BankTransferService()
        {

        }

        public void MakeTransfer(BankAccount accountFrom, BankAccount accountTo, decimal amount)
        {
            accountFrom.Decrease(amount);
            accountTo.Increase(amount);
        }
    }
}
