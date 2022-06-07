using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Banks.Domain
{
    public static class BanksLogEventConsts
    {
        public const int GenerateNewBankAccount = 100;
        public const int TransfersBetweenBankAccounts = 101;
        public const int GetBankAccounts = 102;
    }
}
