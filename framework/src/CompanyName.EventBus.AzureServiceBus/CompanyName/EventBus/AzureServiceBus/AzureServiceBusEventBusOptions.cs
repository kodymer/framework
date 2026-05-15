using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.EventBus.AzureServiceBus
{
    public class AzureServiceBusEventBusOptions
    {
        public string ConnectionString { get; set; }

        public string TopicName { get; set; } = "default";

        public string SubscriberName { get; set; } = "default";
    }
}
