using Microsoft.AspNetCore.Http;

namespace Stubbl.Api.Queries.MatchStubs.Version1
{
    public class Request
    {
        public Request(string httpMethod, string path, IQueryCollection queryStringParameters,
            IHeaderDictionary headers, string body)
        {
            HttpMethod = httpMethod;
            Path = path;
            QueryStringParameters = queryStringParameters;
            Headers = headers;
            Body = body;
        }

        public string Body { get; }
        public IHeaderDictionary Headers { get; }
        public string HttpMethod { get; }
        public string Path { get; }
        public IQueryCollection QueryStringParameters { get; }
    }
}