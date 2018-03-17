using System.Collections.Generic;

namespace Stubbl.Api.Queries.Shared.Version1
{
    public class ResponseLog
    {
        public ResponseLog(int httpStatusCode, string body, IReadOnlyCollection<Header> headers)
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