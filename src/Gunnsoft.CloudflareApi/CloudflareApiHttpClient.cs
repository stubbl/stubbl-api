using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Gunnsoft.CloudflareApi
{
    public class CloudflareApiHttpClient : ICloudflareApiHttpClient
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings;
        private readonly CloudflareApiSettings _cloudflareApiSettings;
        private readonly HttpClient _httpClient;

        static CloudflareApiHttpClient()
        {
            s_jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };
            s_jsonSerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public CloudflareApiHttpClient(CloudflareApiSettings cloudflareApiSettings, HttpClient httpClient)
        {
            _cloudflareApiSettings = cloudflareApiSettings;
            _httpClient = httpClient;
        }

        public async Task DeleteAsync(DeleteRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, BuildRequestUrl(request.PathAndQueryString));

            await SendAsync(requestMessage, cancellationToken);
        }

        public async Task<TResponse> GetAsync<TResponse>(GetRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildRequestUrl(request.PathAndQueryString));

            var responseMessage = await SendAsync(requestMessage, cancellationToken);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public async Task<TResponse> PostAsync<TResponse>(PostRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, BuildRequestUrl(request.PathAndQueryString));
            var content = request.Content == null
                ? "{}"
                : JsonConvert.SerializeObject(request.Content, s_jsonSerializerSettings);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");

            var responseMessage = await SendAsync(requestMessage, cancellationToken);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        private string BuildRequestUrl(string pathAndQueryString)
        {
            var baseUrl = _cloudflareApiSettings.BaseUrl.Trim().TrimEnd('/');

            return $"{baseUrl}/{pathAndQueryString.Trim().TrimStart('/')}";
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage,
            CancellationToken cancellationToken)
        {
            if (!requestMessage.Headers.Contains("X-Auth-Email"))
            {
                requestMessage.Headers.Add("X-Auth-Email", _cloudflareApiSettings.AuthenticationEmailAddress);
            }

            if (!requestMessage.Headers.Contains("X-Auth-Key"))
            {
                requestMessage.Headers.Add("X-Auth-Key", _cloudflareApiSettings.AuthenticationKey);
            }

            var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var response = JObject.Parse(responseJson);
                var errors = response.SelectToken("errors")
                    .ToObject<IReadOnlyCollection<CloudflareApiError>>();

                throw new CloudflareApiException(errors);
            }

            return responseMessage;
        }
    }
}