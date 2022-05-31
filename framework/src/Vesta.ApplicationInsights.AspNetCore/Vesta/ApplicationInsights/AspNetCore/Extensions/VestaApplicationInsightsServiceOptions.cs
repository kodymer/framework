using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace Vesta.ApplicationInsights.AspNetCore.Extensions
{
    public class VestaApplicationInsightsServiceOptions : ApplicationInsightsServiceOptions
    {
        /// <summary>
        /// Gets or sets the role name of component.
        /// </summary>
        public string RoleName { get; set; }
    }
}
