﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.CloudflareApi;
using Gunnsoft.CloudflareApi.DnsRecords.DeleteDnsRecord;
using Gunnsoft.CloudflareApi.DnsRecords.ListDnsRecords;
using Gunnsoft.Cqs.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stubbl.Api.Events.TeamDeleted.Version1;
using Stubbl.Api.Options;

namespace Stubbl.Api.EventHandlers.Cloudflare
{
    public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
    {
        private readonly ICloudflareApi _cloudflareApi;
        private readonly CloudflareOptions _cloudflareOptions;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<TeamDeletedEventHandler> _logger;

        public TeamDeletedEventHandler(ICloudflareApi cloudflareApi, IOptions<CloudflareOptions> cloudflareOptions,
            IHostingEnvironment hostingEnvironment, ILogger<TeamDeletedEventHandler> logger)
        {
            _cloudflareApi = cloudflareApi;
            _cloudflareOptions = cloudflareOptions.Value;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return;
            }

            var listRequest = new ListDnsRecordsRequest
            (
                _cloudflareOptions.ZoneId
            )
            {
                Name = $"{@event.TeamId}.stubbl.in",
                PageNumber = 1,
                PageSize = 1
            };

            var listResponse = await _cloudflareApi.DnsRecords.ListAsync(listRequest, cancellationToken);

            if (!listResponse.DnsRecords.Any())
            {
                _logger.LogInformation
                (
                    "DNS Record not found. TeamID='{TeamID}'",
                    @event.TeamId
                );

                return;
            }

            var dnsRecordId = listResponse.DnsRecords.First().Id;

            var deleteRequest = new DeleteDnsRecordRequest
            (
                _cloudflareOptions.ZoneId,
                dnsRecordId
            );

            await _cloudflareApi.DnsRecords.DeleteAsync(deleteRequest, cancellationToken);
        }
    }
}