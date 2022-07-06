using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.EventBus.Azure
{
    public class AzureEventBusOptionsFactory : IOptionsFactory<AzureEventBusOptions>
    {
        private const string AzureServiceBusDefaultConnectionStringConfig = "Azure:ServiceBus:Connections:Default:ConnectionString";
        private const string AzureEventBusTopicNameConfig = "Azure:EventBus:TopicName";
        private const string AzureEventBusSubscriberNameConfig = "Azure:EventBus:SubscriberName";

        private readonly IConfiguration _configuration;

        public AzureEventBusOptionsFactory(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public AzureEventBusOptions Create(string name)
        {
            var options = new AzureEventBusOptions();

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
