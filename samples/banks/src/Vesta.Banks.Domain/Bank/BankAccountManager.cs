using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vesta.Banks.Bank;

namespace Vesta.Banks.Domain.Bank
{
    public class BankAccountManager : IBankAccountManager
    {
        public BankAccountManager()
        {
        }


        public Task<BankAccount> CreateAsync(decimal initialBalance, CancellationToken cancellationToken = default)
        {
            var bankAccountId = Guid.NewGuid();
            var bankAccountNumber = CreateBankAccountNumber(bankAccountId.ToString());
            var banckAccount = new BankAccount(bankAccountId)
            {
                Number = bankAccountNumber.ToString(),
            };

            banckAccount.AssignOpeningBalance(initialBalance);

            return Task.FromResult(banckAccount);
        }

        private string CreateBankAccountNumber(string id)
        {
            var digits = string.Join("", new Regex(@"\d+").Matches(id));
            var seed = new Regex(@"\d{5}").Match(digits).Value;
            return new Random().Next(Int32.Parse(seed)).ToString();
        }
    }
}
