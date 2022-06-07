﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.EventBus.Azure
{
    public class AzureEventBusOptions
    {
        public string ConnectionString { get; set; }

        public string TopicName { get; set; }

        public string SubscriberName { get; set; }
    }
}
