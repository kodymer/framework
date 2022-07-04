using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountChangedEventHandler : IIntegrationEventHandler<BankAccountChangedEto>
    {
        public ILogger<BankAccountChangedEventHandler> Logger { get; set; }


        public BankAccountChangedEventHandler()
        {
            // Inject repositories or applications API

            Logger = NullLogger<BankAccountChangedEventHandler>.Instance;
        }


        public Task HandleEventAsync(BankAccountChangedEto args)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
