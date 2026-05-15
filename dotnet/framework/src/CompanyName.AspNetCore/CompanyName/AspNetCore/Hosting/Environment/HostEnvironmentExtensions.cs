using Microsoft.Extensions.Hosting;

namespace CompanyName.AspNetCore.Hosting.Environment
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsKubernetes(this IHostEnvironment env)
            => HostingEnvironmentHelper.IsKubernetes();
    }
}
