namespace Gunnsoft.CloudflareApi.DnsRecords
{
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using System.Linq;
   using System.Net;
   using System.Threading;
   using System.Threading.Tasks;
   using DnsRecords.CreateDnsRecord;
   using Gunnsoft.CloudflareApi.DnsRecords.DeleteDnsRecord;
   using Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords;

   public class HttpClientDnsRecords : IDnsRecords
   {
      private readonly ICloudflareApiHttpClient _httpClient;

      public HttpClientDnsRecords(ICloudflareApiHttpClient httpClient)
      {
         _httpClient = httpClient;
      }

      public async Task<CreateDnsRecordResponse> CreateAsync(CreateDnsRecordRequest request, CancellationToken cancellationToken = default(CancellationToken))
      {
         var content = new
         {
            request.Content,
            request.Name,
            request.Proxied,
            Ttl = request.TimeToLive,
            request.Type,
         };
         var postRequest = new PostRequest($"zones/{request.ZoneId}/dns_records", content);

         return await _httpClient.PostAsync<CreateDnsRecordResponse>(postRequest, cancellationToken);
      }

      public async Task DeleteAsync(DeleteDnsRecordRequest request, CancellationToken cancellationToken)
      {
         var deleteRequest = new DeleteRequest($"zones/{request.ZoneId}/dns_records/{request.DnsRecordId}");

         await _httpClient.DeleteAsync(deleteRequest, cancellationToken);
      }

      public async Task<ListDnsRecordsResponse> ListAsync(ListDnsRecordsRequest request, CancellationToken cancellationToken)
      {
         var pathAndQueryString = $"zones/{request.ZoneId}/dns_records";
         var queryString = new NameValueCollection();

         if (request.Type != null)
         {
            queryString.Add("type", request.Type.ToString());
         }

         if (request.Name != null)
         {
            queryString.Add("name", request.Name);
         }

         if (request.Content != null)
         {
            queryString.Add("content", request.Content);
         }

         if (request.PageNumber != null)
         {
            queryString.Add("page", request.PageNumber.ToString());
         }

         if (request.PageSize != null)
         {
            queryString.Add("per_page", request.PageSize.ToString());
         }

         if (request.SortField != null)
         {
            queryString.Add("order", request.SortField.ToString().ToLower());
         }

         if (request.SortDirection != null)
         {
            queryString.Add("direction", request.SortDirection.ToString().ToLower());
         }

         if (request.Match != null)
         {
            queryString.Add("match", request.Match.ToString().ToLower());
         }

         if (queryString.Count > 0)
         {
            pathAndQueryString += $"?{string.Join("&", queryString.AllKeys.Select(key => $"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(queryString[key])}"))}";
         }

         var getRequest = new GetRequest(pathAndQueryString);

         return await _httpClient.GetAsync<ListDnsRecordsResponse>(getRequest, cancellationToken);
      }
   }
}
