using CompanyName.ProjectName.EntityFrameworkCore;
using MassTransit;

namespace CompanyName.ProjectName
{

    public static class BusProjectNameHttpApiHost
    {

        internal static IServiceCollection AddProjectNameBus(this IServiceCollection services, IConfiguration configuration)
        {

            Action<IBusRegistrationConfigurator> busConfigurator = options =>
            {
                options.AddConsumers(typeof(ProjectNameApplication).Assembly);

                options.UsingRabbitMq((context, config) =>
                {
                    config.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    /* To avoid duplicate messages in 
                     * the event of a concurrency failure
                     * See https://masstransit.io/documentation/patterns/saga/persistence#publishing-and-sending-from-sagas
                     */

                    config.UseInMemoryOutbox(context);

                    /*
                     * ConfigureEndpoints should be the last method 
                     * called after all settings and middleware 
                     * components have been configured.
                     */
                    config.ConfigureEndpoints(context);
                });

                options.AddEntityFrameworkOutbox<ProjectNameDbContext>(options =>
                {
                    options
                        .UseSqlServer()
                        .UseBusOutbox();
                });
            };

            Action<IMediatorRegistrationConfigurator> mediatorConfigurator = options =>
            {
                options.AddConsumers(typeof(ProjectNameApplication).Assembly);
            };

            // NOTE:
            // Uncomment the following line ONLY if you need to enable MassTransit for message bus integration.
            // This is typically required when the microservice must publish or consume events via RabbitMQ.
            // IMPORTANT: If you enable MassTransit here, also uncomment the RabbitMQ health check registration
            // in the HealthCheck configuration to ensure Kubernetes readiness probes reflect the bus status.

            services
                .AddCompanyNameBus(busConfigurator: false ? busConfigurator : null, mediatorConfigurator);

            return services;
        }
    }
}
