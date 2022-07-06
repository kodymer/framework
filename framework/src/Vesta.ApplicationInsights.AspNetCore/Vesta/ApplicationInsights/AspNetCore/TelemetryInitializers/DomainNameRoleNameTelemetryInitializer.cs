using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Ardalis.GuardClauses;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Vesta.ApplicationInsights.AspNetCore.Extensions;

namespace Vesta.ApplicationInsights.AspNetCore.TelemetryInitializers
{
 
    /// <summary>
    /// A telemetry initializer that populates cloud context role name.
    /// </summary>
    public class DomainNameRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string _roleName;

        public DomainNameRoleNameTelemetryInitializer(IOptions<VestaApplicationInsightsServiceOptions> options)
        {
            _roleName = options.Value.RoleName;
        }

        /// <summary>
        /// Initializes role name and node name with the host name.
        /// </summary>
        /// <param name="telemetry">Telemetry item.</param>
        public void Initialize(ITelemetry telemetry)
        {

             Guard.Against.Null(telemetry, nameof(telemetry));
            

            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = _roleName;
            }
        }
    }
}
