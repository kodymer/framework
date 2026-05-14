<<<<<<< HEAD:samples/banks/src/CompanyName.Banks.Domain/Banks/BankAccountPublisher.cs
﻿using CompanyName.Ddd.Domain.Services;
using CompanyName.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
=======
﻿using System.Text.Json;
using CompanyName.Ddd.Domain.Services;
using CompanyName.Eventing.Abstractions;
using Microsoft.Extensions.Logging;
>>>>>>> develop:samples/banks/src/Samples.Banks.Domain/Accounts/BankAccountPublisher.cs

namespace Samples.Banks.Accounts
{
    public class BankAccountPublisher : DomainService, IBankAccountPublisher
    {

        private readonly ILocalEventBus _eventBus;

        public BankAccountPublisher(ILocalEventBus eventBus)
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
                Logger.LogError(innerException, "Error publishing the bank account: {BankAccount}", JsonSerializer.Serialize(account));

                throw new ReportingBankAccountException(
                    $"Error publishing the bank account ({account.Number}). See the inner exception for more details.", innerException);
            }
        }
    }
}
