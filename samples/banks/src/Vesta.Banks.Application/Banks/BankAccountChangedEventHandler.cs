using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;
using Vesta.Security.Users;

namespace Vesta.Banks
{
    public class BankAccountChangedEventHandler : IIntegrationEventHandler<BankAccountChangedEto>
    {
        private readonly ICurrentUser _user;

        public ILogger<BankAccountChangedEventHandler> Logger { get; set; }


        public BankAccountChangedEventHandler(ICurrentUser user)
        {
            // Inject repositories or applications API

            Logger = NullLogger<BankAccountChangedEventHandler>.Instance;
            
            _user = user;
        }

        public Task HandleEventAsync(BankAccountChangedEto args)
        {           

            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            Logger.LogDebug("Current user: ", JsonSerializer.Serialize(_user));

            return Task.CompletedTask;
        }
    }
}
