using Vesta.Ddd.Domain.Services;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks.Bank
{
    public class BankTransferService : DomainService, IBankTransferService
    {
        private readonly IDistributedEventBus _eventBus;

        public BankTransferService(IDistributedEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<BankTransfer> MakeTransferAsync(BankAccount accountFrom, BankAccount accountTo, decimal amount)
        {

            accountFrom.Decrease(amount);
            accountTo.Increase(amount);

            var bankTransfer = new BankTransfer(accountFrom.Id, accountTo.Id, amount);
            accountFrom.Debits.Add(bankTransfer);
            accountTo.Credits.Add(bankTransfer);

            _eventBus.PublishAsync(accountFrom);
            _eventBus.PublishAsync(accountTo);

            return Task.FromResult(bankTransfer);
        }
    }
}
