using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vesta.EventBus.Abstracts;

namespace Vesta.EventBus.Hosting
{
    public class EventBusInitializer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public EventBusInitializer( 
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var logger = _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<EventBusInitializer>();

            if (_serviceProvider.GetService<ILocalEventBus>() is null)
            {
                logger.LogWarning("Could not initialize local event bus service. You must check the configuration.");
            };

            if (_serviceProvider.GetService<IDistributedEventBus>() is null)
            {
                logger.LogWarning("Could not initialize distribute event bus service. You must check the configuration.");
            };

            logger.LogInformation("Event bus services started!");

            return Task.CompletedTask;
        }
    }
}
