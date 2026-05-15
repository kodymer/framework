using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.Banks
{
    public static class BanksLogEventConsts
    {
        // General
        public const int General = 1000;

        // Bank Transfer Events
        public const int GetBankTransferHistory = 2000;
        public const int GenerateNewBankAccount = 2001;
        public const int TransfersBetweenBankAccounts = 2002;

        // Bank Account Events
        public const int GetBankAccounts = 3000;
    }
}
