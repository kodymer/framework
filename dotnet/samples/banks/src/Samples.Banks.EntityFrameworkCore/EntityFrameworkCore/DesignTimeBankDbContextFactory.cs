using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Samples.Banks.EntityFrameworkCore
{
    public class DesignTimeBankDbContextFactory : IDesignTimeDbContextFactory<BankDbContext>
    {
        public BankDbContext CreateDbContext(string[] args)
        {
            var environmentName =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? "Production";

            bool isDevelopment = string.Equals(environmentName, "Development", StringComparison.OrdinalIgnoreCase);

            var basePath = Directory.GetCurrentDirectory();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true);

            if (isDevelopment)
            {
                configBuilder.AddUserSecrets<DesignTimeBankDbContextFactory>(optional: true);
            }

            configBuilder
                .AddEnvironmentVariables();

            if (args is { Length: > 0 })
            {
                configBuilder = configBuilder
                    .AddCommandLine(args);
            }

            var config = configBuilder.Build();


            var connectionString =
                config.GetConnectionString("Default")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? "Host=localhost;Database=Samples.BankDb;Username=guest;Password=guest"; // fallback dev

            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseSqlServer(connectionString)   
                .Options;

            return new BankDbContext(options);
        }
    }

}
