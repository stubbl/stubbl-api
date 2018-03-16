namespace Stubbl.Api.Core.EventHandlers.Cloudflare
{
    using CodeContrib.CloudflareApi;
    using CodeContrib.CloudflareApi.DnsRecords;
    using CodeContrib.CloudflareApi.DnsRecords.CreateDnsRecord;
    using CodeContrib.EventHandlers;
    using Events.TeamCreated.Version1;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using Options;
    using System.Threading;
    using System.Threading.Tasks;

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
            if (_hostingEnvironment.IsDevelopment())
            {
                return;
            }

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