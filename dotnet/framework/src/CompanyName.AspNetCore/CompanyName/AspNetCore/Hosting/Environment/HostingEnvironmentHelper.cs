namespace CompanyName.AspNetCore.Hosting.Environment
{
    public static class HostingEnvironmentHelper
    {
        /// <summary>
        /// Determines whether the application is running inside a Kubernetes cluster.
        /// </summary>
        public static bool IsKubernetes()
        {
            var kubernetesServiceHost = System.Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST");
            var serviceAccountTokenPath = "/var/run/secrets/kubernetes.io/serviceaccount/token";

            return !string.IsNullOrEmpty(kubernetesServiceHost) || File.Exists(serviceAccountTokenPath);
        }
    }
}
