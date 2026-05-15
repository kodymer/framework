using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Samples.Banks.AccountManagement
{
    public class BankAccountCreatedEventHandler : IEventHandler<BankAccountCreatedEvent>
    {
        private readonly IAccountManagementAppService _bankAppService;

        public ILogger<BankAccountCreatedEventHandler> Logger { get; set; }


        public BankAccountCreatedEventHandler(IAccountManagementAppService bankAppService)
        {
            // Inject repositories or applications API

            _bankAppService = bankAppService;
        }

        public async Task HandleAsync(BankAccountCreatedEvent args, CancellationToken cancellationToken = default)
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
