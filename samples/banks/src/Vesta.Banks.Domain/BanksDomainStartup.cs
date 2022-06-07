using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vesta.Banks.Configuration;

namespace Vesta.Banks
{
    public static class BanksDomainStartup
    {

        private const string AzureServiceBusConnectionStringConfig = "Azure:ServiceBus:Connections:Default:ConnectionString";
        private const string AzureServiceBusTopicNameConfig = "Azure:EventBus:TopicName";
        private const string AzureServiceBusSubscriberNameConfig = "Azure:EventBus:SubscriberName";

        public static void AddBanksDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBanksDomainShared(configuration);

            services.AddBanksDomainSevices();

            services.AddVestaEventBusAzure(options =>
            {
                options.ConnectionString = configuration.GetValue<string>(AzureServiceBusConnectionStringConfig);
                options.TopicName = configuration.GetValue<string>(AzureServiceBusTopicNameConfig);
                options.SubscriberName = configuration.GetValue<string>(AzureServiceBusSubscriberNameConfig);

            });
        }
    }
}
