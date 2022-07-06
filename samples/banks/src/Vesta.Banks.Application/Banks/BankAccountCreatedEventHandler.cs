using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountCreatedEventHandler : IIntegrationEventHandler<BankAccountCreatedEto>
    {
        public ILogger<BankAccountCreatedEventHandler> Logger { get; set; }


        public BankAccountCreatedEventHandler()
        {
            // Inject repositories or applications API

            Logger = NullLogger<BankAccountCreatedEventHandler>.Instance;
        }


        public Task HandleEventAsync(BankAccountCreatedEto args)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
