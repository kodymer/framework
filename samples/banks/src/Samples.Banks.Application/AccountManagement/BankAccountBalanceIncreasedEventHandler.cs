using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountBalanceIncreasedEventHandler : CompanyName.Messaging.Abstractions.EventHandler<BankAccountBalanceIncreasedEvent>
    {
        public override Task HandleAsync(BankAccountBalanceIncreasedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }

}
