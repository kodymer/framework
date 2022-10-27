using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Vesta.Banks.Etos;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountCreatedEventHandler : IIntegrationEventHandler<BankAccountCreatedEto>
    {
        private readonly IBankAppService _bankAppService;

        public ILogger<BankAccountCreatedEventHandler> Logger { get; set; }


        public BankAccountCreatedEventHandler(IBankAppService bankAppService)
        {
            // Inject repositories or applications API

            _bankAppService = bankAppService;

            Logger = NullLogger<BankAccountCreatedEventHandler>.Instance;
        }


        public async Task HandleEventAsync(BankAccountCreatedEto args)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            try
            {
               var bankAccounts = await _bankAppService.GetAllBankAccountListAsync();
            }
            catch (Exception e)
            {

                Logger.LogError(e, "Error");
            }
            //return Task.CompletedTask;
        }
    }
}
