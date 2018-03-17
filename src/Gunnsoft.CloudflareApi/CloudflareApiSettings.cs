namespace Gunnsoft.CloudflareApi
{
    using System;

    public class CloudflareApiSettings
    {
        public CloudflareApiSettings(string baseUrl, string authenticationKey, string authenticationEmailAddress)
        {
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            AuthenticationKey = authenticationKey ?? throw new ArgumentNullException(nameof(authenticationKey));
            AuthenticationEmailAddress = authenticationEmailAddress ??
                                         throw new ArgumentNullException(nameof(authenticationEmailAddress));
        }

        public string BaseUrl { get; }
        public string AuthenticationEmailAddress { get; }
        public string AuthenticationKey { get; }
    }
}