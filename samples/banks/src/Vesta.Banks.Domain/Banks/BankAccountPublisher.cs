using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Services;
using Vesta.EventBus.Abstracts;

namespace Vesta.Banks
{
    public class BankAccountPublisher : DomainService, IBankAccountPublisher
    {


        private readonly IDistributedEventBus _eventBus;

        public BankAccountPublisher(IDistributedEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task PublishAsync(BankAccount account, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Publish bank account: {BankAccount}", JsonSerializer.Serialize(account));

            try
            {
                await _eventBus.PublishAsync(account);
            }
            catch (Exception innerException)
            {
                Logger.LogError("Error publishing the bank account: {BankAccount}", JsonSerializer.Serialize(account));

                throw new ReportingBankAccountException(
                    $"Error publishing the bank account ({account.Number}). See the inner exception for more details.", innerException);
            }

        }


    }
}
