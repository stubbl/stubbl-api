namespace Stubbl.Api.Core.EventHandlers.Cloudflare
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.CloudflareApi;
   using Common.CloudflareApi.DnsRecords;
   using Common.CloudflareApi.DnsRecords.CreateDnsRecord;
   using Common.EventHandlers;
   using Events.TeamCreated.Version1;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.Extensions.Options;
   using MongoDB.Driver;

   public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
   {
      private readonly ICloudflareApi _cloudflareApi;
      private readonly CloudflareOptions _cloudflareOptions;
      private readonly IHostingEnvironment _hostingEnvironment;

      public TeamCreatedEventHandler(ICloudflareApi cloudflareApi, IOptions<CloudflareOptions> cloudflareOptions,
         IHostingEnvironment hostingEnvironment)
      {
         _cloudflareApi = cloudflareApi;
         _cloudflareOptions = cloudflareOptions.Value;
         _hostingEnvironment = hostingEnvironment;
      }

      public async Task HandleAsync(TeamCreatedEvent @event, CancellationToken cancellationToken)
      {
         //if (_hostingEnvironment.IsDevelopment())
         //{
         //   return;
         //}

         var request = new CreateDnsRecordRequest
         (
            _cloudflareOptions.ZoneId,
            $"{@event.TeamId}.stubbl.in",
            DnsRecordType.CNAME,
            "stubblapi.azurewebsites.net"
         );

         var response = await _cloudflareApi.DnsRecords.CreateAsync(request, cancellationToken);
      }
   }
}