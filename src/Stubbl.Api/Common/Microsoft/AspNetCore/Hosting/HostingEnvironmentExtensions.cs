namespace Microsoft.AspNetCore.Hosting
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsIntegrationTesting(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment("IntegrationTesting");
        }
    }
}
