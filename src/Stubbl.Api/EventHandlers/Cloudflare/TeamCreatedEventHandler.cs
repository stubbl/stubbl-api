using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.CloudflareApi;
using Gunnsoft.CloudflareApi.DnsRecords;
using Gunnsoft.CloudflareApi.DnsRecords.CreateDnsRecord;
using Gunnsoft.Cqs.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Stubbl.Api.Events.TeamCreated.Version1;
using Stubbl.Api.Options;

namespace Stubbl.Api.EventHandlers.Cloudflare
{
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
                DnsRecordType.Cname,
                "stubblapi.azurewebsites.net"
            );

            var response = await _cloudflareApi.DnsRecords.CreateAsync(request, cancellationToken);
        }
    }
}