using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountBalanceDecreasedEventHandler : IEventHandler<BankAccountBalanceDecreasedEvent>
    {
        public ILogger<BankAccountBalanceDecreasedEventHandler> Logger { get; set; }

        public BankAccountBalanceDecreasedEventHandler()
        {
            Logger = NullLogger<BankAccountBalanceDecreasedEventHandler>.Instance;
        }

        public Task HandleAsync(BankAccountBalanceDecreasedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
