using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountBalanceIncreasedEventHandler : IEventHandler<BankAccountBalanceIncreasedEvent>
    {
        public ILogger<BankAccountBalanceIncreasedEventHandler> Logger { get; set; }

        public BankAccountBalanceIncreasedEventHandler()
        {
            Logger = NullLogger<BankAccountBalanceIncreasedEventHandler>.Instance;
        }

        public Task HandleAsync(BankAccountBalanceIncreasedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }

}
