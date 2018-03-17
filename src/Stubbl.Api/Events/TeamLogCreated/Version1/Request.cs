using System.Collections.Generic;
using Stubbl.Api.Events.Shared.Version1;

namespace Stubbl.Api.Events.TeamLogCreated.Version1
{
    public class Request
    {
        public Request(string httpMethod, string path, IReadOnlyCollection<QueryStringParameter> queryStringParameters,
            string body, IReadOnlyCollection<Header> headers)
        {
            HttpMethod = httpMethod;
            Path = path;
            QueryStringParameters = queryStringParameters;
            Body = body;
            Headers = headers;
        }

        public string Body { get; }
        public IReadOnlyCollection<Header> Headers { get; }
        public string HttpMethod { get; }
        public string Path { get; }
        public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; }
    }
}