namespace Microsoft.AspNetCore.Hosting
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsIntegrationTesting(this IHostingEnvironment extended)
        {
            return extended.IsEnvironment("IntegrationTesting");
        }
    }
}
