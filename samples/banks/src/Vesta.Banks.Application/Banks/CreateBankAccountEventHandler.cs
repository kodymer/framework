using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vesta.Banks;
using Vesta.Banks.Application;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class CreateBankAccountEventHandler : IDistributedEventHandler<BankAccount>
    {
        public ILogger<CreateBankAccountEventHandler> Logger { get; set; }

        public CreateBankAccountEventHandler()
        {
            Logger = NullLogger<CreateBankAccountEventHandler>.Instance;
        }


        public Task HandleEventAsync(BankAccount args)
        {
            Logger.LogInformation("Llegó la cuenta {Message}:", JsonSerializer.Serialize(args));

            return Task.CompletedTask;
        }
    }
}
