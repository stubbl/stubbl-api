using System.Collections.Generic;
using Stubbl.Api.Commands.Shared.Version1;

namespace Stubbl.Api.Commands.CreateTeamLog.Version1
{
    public class Response
    {
        public Response(int httpStatusCode, string body, IReadOnlyCollection<Header> headers)
        {
            HttpStatusCode = httpStatusCode;
            Body = body;
            Headers = headers;
        }

        public string Body { get; }
        public IReadOnlyCollection<Header> Headers { get; }
        public int HttpStatusCode { get; }
    }
}