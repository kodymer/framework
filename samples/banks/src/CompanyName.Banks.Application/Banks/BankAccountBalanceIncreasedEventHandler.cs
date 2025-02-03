using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountBalanceIncreasedEventHandler : IDomainEventHandler<BankAccountBalanceIncreasedEto>
    {

        public ILogger<BankAccountBalanceIncreasedEventHandler> Logger { get; set; }

        public BankAccountBalanceIncreasedEventHandler()
        {
            Logger = NullLogger<BankAccountBalanceIncreasedEventHandler>.Instance;
        }

        public Task HandleEventAsync(BankAccountBalanceIncreasedEto args)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }

}
