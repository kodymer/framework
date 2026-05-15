using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Data;

namespace CompanyName.Dapper
{
    public class DatabaseOptionsFactory : IOptionsFactory<DatabaseOptions>
    {
        private readonly IConfiguration _configuration;

        private string ConnectionStringName { get; set; } 

        public DatabaseOptionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnectionStringName = ConnectionStrings.DefaultNameConfig;
        }

        public DatabaseOptions Create(string name)
        {
            var options = new DatabaseOptions();

            var connectionString = _configuration.GetConnectionString(ConnectionStringName);
            if(!string.IsNullOrWhiteSpace(connectionString))
            {
                options.ConnectionString = connectionString;
            }

            return options;
        }

        internal void SetConnectionStringName(string connectionStringName = ConnectionStrings.DefaultNameConfig)
        {
            ConnectionStringName = connectionStringName;
        }
    }
}
