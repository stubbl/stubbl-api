using System.Collections.Generic;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data.Collections.Logs
{
    public class Response
    {
        public Response()
        {
            Headers = new Header[0];
        }

        public string Body { get; set; }
        public IReadOnlyCollection<Header> Headers { get; set; }
        public int HttpStatusCode { get; set; }
    }
}