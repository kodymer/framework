using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace CompanyName.ApplicationInsights.AspNetCore.Extensions
{
    public class CompanyNameApplicationInsightsServiceOptions : ApplicationInsightsServiceOptions
    {
        /// <summary>
        /// Gets or sets the role name of component.
        /// </summary>
        public string RoleName { get; set; }
    }
}
