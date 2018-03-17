using System.Collections.Generic;

namespace Stubbl.Api.Events.Shared.Version1
{
    public class Request
    {
        public Request(string httpMethod, string path, IReadOnlyCollection<QueryStringParameter> queryStringParameters,
            IReadOnlyCollection<BodyToken> bodyTokens, IReadOnlyCollection<Header> headers)
        {
            HttpMethod = httpMethod;
            Path = path;
            QueryStringParameters = queryStringParameters;
            BodyTokens = bodyTokens;
            Headers = headers;
        }

        public IReadOnlyCollection<BodyToken> BodyTokens { get; }
        public IReadOnlyCollection<Header> Headers { get; }
        public string HttpMethod { get; }
        public string Path { get; }
        public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; }
    }
}