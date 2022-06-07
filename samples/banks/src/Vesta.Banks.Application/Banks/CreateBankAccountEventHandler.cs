using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class CreateBankAccountEventHandler : IDistributedEventHandler<BankAccountEto>
    {
        public ILogger<CreateBankAccountEventHandler> Logger { get; set; }

        public CreateBankAccountEventHandler()
        {
            Logger = NullLogger<CreateBankAccountEventHandler>.Instance;
        }


        public Task HandleEventAsync(BankAccountEto args)
        {
            Logger.LogInformation("Llegó la cuenta {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
