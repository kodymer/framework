using CompanyName.EventBus.Abstractions;
using CompanyName.Security.Users;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountChangedEventHandler : IEventHandler<BankAccountChangedEvent>
    {
        private readonly ICurrentUser _user;

        public ILogger<BankAccountChangedEventHandler> Logger { get; set; }


        public BankAccountChangedEventHandler(ICurrentUser user)
        {
            // Inject repositories or applications API

            _user = user;
        }

        public Task HandleAsync(BankAccountChangedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            Logger.LogDebug("Current user: {User}", JsonSerializer.Serialize(_user));

            return Task.CompletedTask;
        }
    }
}
