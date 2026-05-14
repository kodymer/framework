using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountCreatedEventHandler : CompanyName.Messaging.Abstractions.EventHandler<BankAccountCreatedEvent>
    {
        private readonly IAccountManagementAppService _bankAppService;

        public BankAccountCreatedEventHandler(IAccountManagementAppService bankAppService)
        {
            // Inject repositories or applications API

            _bankAppService = bankAppService;
        }

        public override async Task HandleAsync(BankAccountCreatedEvent args, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Message received: {Message}:", JsonSerializer.Serialize(args));

            try
            {
                await _bankAppService.DoSomething();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error");
            }
        }
    }
}
