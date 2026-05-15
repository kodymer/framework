using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.EventBus.AzureServiceBus
{
    public class AzureServiceBusEventBusOptionsFactory : IOptionsFactory<AzureServiceBusEventBusOptions>
    {
        private const string AzureServiceBusDefaultConnectionStringConfig = "Azure:ServiceBus:Connections:Default:ConnectionString";
        private const string AzureEventBusTopicNameConfig = "Azure:EventBus:TopicName";
        private const string AzureEventBusSubscriberNameConfig = "Azure:EventBus:SubscriberName";

        private readonly IConfiguration _configuration;

        public AzureServiceBusEventBusOptionsFactory(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public AzureServiceBusEventBusOptions Create(string name)
        {
            var options = new AzureServiceBusEventBusOptions();

            var eventBusConnection = _configuration.GetValue<string>(AzureServiceBusDefaultConnectionStringConfig);
            if (!string.IsNullOrWhiteSpace(eventBusConnection))
            {
                options.ConnectionString = eventBusConnection;
            }

            var eventBusTopicName = _configuration.GetValue<string>(AzureEventBusTopicNameConfig);
            if (!string.IsNullOrWhiteSpace(eventBusTopicName))
            {
                options.TopicName = eventBusTopicName;
            }

            var eventBusSubscriberName = _configuration.GetValue<string>(AzureEventBusSubscriberNameConfig);
            if (!string.IsNullOrWhiteSpace(eventBusSubscriberName))
            {
                options.SubscriberName = eventBusSubscriberName;
            }

            return options;
        }
    }
}
