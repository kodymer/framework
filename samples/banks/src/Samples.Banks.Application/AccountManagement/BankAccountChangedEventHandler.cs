using CompanyName.Security.Users;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountChangedEventHandler : CompanyName.Messaging.Abstractions.EventHandler<BankAccountChangedEvent>
    {

        public BankAccountChangedEventHandler(ICurrentUser user)
        {
            // Inject repositories or applications API
        }

        public override Task HandleAsync(BankAccountChangedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            Logger.LogDebug("Current user: {User}", JsonSerializer.Serialize(CurrenUser));

            return Task.CompletedTask;
        }
    }
}
