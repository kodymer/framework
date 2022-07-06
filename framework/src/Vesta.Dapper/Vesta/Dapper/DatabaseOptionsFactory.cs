using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Data;

namespace Vesta.Dapper
{
    public class DatabaseOptionsFactory : IOptionsFactory<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;

        public DatabaseOptionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DatabaseOptions Create(string name)
        {
            var options = new DatabaseOptions();

            var connectionString = _configuration.GetConnectionString(ConnectionStrings.DefaultNameConfig);
            if(!string.IsNullOrWhiteSpace(connectionString))
            {
                options.ConnectionString = connectionString;
            }

            return options;
        }
    }
}
