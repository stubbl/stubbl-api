namespace Stubbl.Api.Common.CloudflareApi
{
   using System;
   using System.Collections.Generic;
   using System.Net.Http;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;
   using Newtonsoft.Json;
   using Newtonsoft.Json.Converters;
   using Newtonsoft.Json.Linq;
   using Newtonsoft.Json.Serialization;

   public class CloudflareApiHttpClient : ICloudflareApiHttpClient
   {
      private readonly CloudflareApiOptions _cloudflareApiOptions;
      private readonly HttpClient _httpClient;
      private static readonly JsonSerializerSettings _jsonSerializerSettings;

      static CloudflareApiHttpClient()
      {
         _jsonSerializerSettings = new JsonSerializerSettings
         {
            ContractResolver = new DefaultContractResolver
            {
               NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
         };
         _jsonSerializerSettings.Converters.Add(new StringEnumConverter());
      }

      public CloudflareApiHttpClient(HttpClient httpClient, CloudflareApiOptions cloudflareApiOptions)
      {
         _httpClient = httpClient;
         _cloudflareApiOptions = cloudflareApiOptions;
      }

      private string BuildRequestUrl(string pathAndQueryString)
      {
         var baseUrl = _cloudflareApiOptions.BaseUrl.Trim().TrimEnd('/');

         return $"{baseUrl}/{pathAndQueryString.Trim().TrimStart('/')}";
      }

      public async Task DeleteAsync(DeleteRequest request, CancellationToken cancellationToken = default(CancellationToken))
      {
         var requestMessage = new HttpRequestMessage(HttpMethod.Delete, BuildRequestUrl(request.PathAndQueryString));

         await SendAsync(requestMessage, cancellationToken);
      }

      public async Task<TResponse> GetAsync<TResponse>(GetRequest request, CancellationToken cancellationToken = default(CancellationToken))
      {
         var requestMessage = new HttpRequestMessage(HttpMethod.Get, BuildRequestUrl(request.PathAndQueryString));

         var responseMessage = await SendAsync(requestMessage, cancellationToken);
         var responseJson = await responseMessage.Content.ReadAsStringAsync();

         return JsonConvert.DeserializeObject<TResponse>(responseJson);
      }

      public async Task<TResponse> PostAsync<TResponse>(PostRequest request, CancellationToken cancellationToken = default(CancellationToken))
      {
         var requestMessage = new HttpRequestMessage(HttpMethod.Post, BuildRequestUrl(request.PathAndQueryString));
         var content = request.Content == null ? "{}" : JsonConvert.SerializeObject(request.Content, _jsonSerializerSettings);
         requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");

         var responseMessage = await SendAsync(requestMessage, cancellationToken);
         var responseJson = await responseMessage.Content.ReadAsStringAsync();

         return JsonConvert.DeserializeObject<TResponse>(responseJson);
      }

      private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
      {
         if (!requestMessage.Headers.Contains("X-Auth-Email"))
         {
            requestMessage.Headers.Add("X-Auth-Email", _cloudflareApiOptions.AuthenticationEmailAddress);
         }

         if (!requestMessage.Headers.Contains("X-Auth-Key"))
         {
            requestMessage.Headers.Add("X-Auth-Key", _cloudflareApiOptions.AuthenticationKey);
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
