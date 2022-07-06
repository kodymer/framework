using Vesta.Banks;
using Vesta.Ddd.Domain.Services;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
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

            var bankTransfer = new BankTransfer(accountFrom.Number, accountTo.Number, amount);
            return Task.FromResult(bankTransfer);
        }
    }
}
