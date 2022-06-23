using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Caching.StackExchangeRedis
{
    public class RedisCacheOptionsFactory : IOptionsFactory<RedisCacheOptions>
    {
        private const string RedisCacheConfigurationConfig = "Redis:Configuration";
        private const string RedisCacheInstanceNameConfig = "Redis:InstanceName";

        private readonly IConfiguration _configuration;

        public RedisCacheOptionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RedisCacheOptions Create(string name)
        {
            var options = new RedisCacheOptions();

            var redisConfiguration = _configuration.GetValue<string>(RedisCacheConfigurationConfig);
            if (!string.IsNullOrWhiteSpace(redisConfiguration))
            {
                options.Configuration = redisConfiguration;
            }

            var redisInstanceName = _configuration.GetValue<string>(RedisCacheInstanceNameConfig);
            if (!string.IsNullOrWhiteSpace(redisInstanceName))
            {
                options.InstanceName = redisInstanceName;
            }

            return options;
        }
    }
}
