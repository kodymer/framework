using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountBalanceDecreasedEventHandler : CompanyName.Messaging.Abstractions.EventHandler<BankAccountBalanceDecreasedEvent>
    {
        public override Task HandleAsync(BankAccountBalanceDecreasedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
