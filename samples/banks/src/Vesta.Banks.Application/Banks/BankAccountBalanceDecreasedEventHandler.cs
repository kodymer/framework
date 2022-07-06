using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountBalanceDecreasedEventHandler : IDomainEventHandler<BankAccountBalanceDecreasedEto>
    {

        public ILogger<BankAccountBalanceDecreasedEventHandler> Logger { get; set; }

        public BankAccountBalanceDecreasedEventHandler()
        {
            Logger = NullLogger<BankAccountBalanceDecreasedEventHandler>.Instance;
        }

        public Task HandleEventAsync(BankAccountBalanceDecreasedEto args)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }

}
