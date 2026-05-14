<<<<<<< HEAD:samples/banks/src/CompanyName.Banks.Domain/Banks/BankTransferService.cs
﻿using CompanyName.Banks;
using CompanyName.Ddd.Domain.Services;
using CompanyName.EventBus.Abstractions;
=======
﻿using CompanyName.Ddd.Domain.Services;
using Samples.Banks.Accounts;
>>>>>>> develop:samples/banks/src/Samples.Banks.Domain/Transfers/BankTransferService.cs

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
