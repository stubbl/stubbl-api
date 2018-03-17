using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace System.Net.Http
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(HttpMessageHandler innerHandler, ILogger<LoggingHandler> logger)
            : base(innerHandler)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Content == null)
            {
                _logger.LogInformation
                (
                    "Sending {RequestMethod} request to URL {RequestUrl} with headers {@Headers}",
                    request.Method,
                    request.RequestUri,
                    request.Headers
                );
            }
            else
            {
                _logger.LogInformation
                (
                    "Sending {RequestMethod} request to URL {RequestUrl} with headers {@Headers} and content {RequestContent}",
                    request.Method,
                    request.RequestUri,
                    request.Headers,
                    await request.Content.ReadAsStringAsync()
                );
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content == null)
            {
                _logger.LogInformation
                (
                    "Received response for with HTTP status code {ResponseHttpStatusCode}",
                    response.StatusCode
                );
            }
            else
            {
                _logger.LogInformation
                (
                    "Received response for with HTTP status code {ResponseHttpStatusCode} and content {@ResponseContent}",
                    response.StatusCode,
                    await response.Content.ReadAsStringAsync()
                );
            }

            return response;
        }
    }
}