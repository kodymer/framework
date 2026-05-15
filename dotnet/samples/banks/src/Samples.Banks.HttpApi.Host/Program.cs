using Samples.Banks;
using Serilog;
using Serilog.Events;

bool IsDesignTime() =>
    AppDomain.CurrentDomain.GetAssemblies()
        .Any(a => a.GetName().Name?.StartsWith("Microsoft.EntityFrameworkCore.Design") == true);

if (IsDesignTime())
    return 0;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting CompanyName.Banks.HttpApi.Host.");

    var builder = WebApplication.CreateBuilder(args);
    var host = builder.Host;
    var enviroment = builder.Environment;
    var services = builder.Services;
    var configuration = builder.Configuration;

    host
        .ConfigureBanksHttpApiHost(configuration);

    services
        .AddBanksHttpApiHost(enviroment, configuration);

    var app = builder.Build();

    app
        .UseBanksHttpApiHost();

    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");

    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}