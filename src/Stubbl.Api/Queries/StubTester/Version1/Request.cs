namespace Stubbl.Api.Queries.StubTester.Version1
{
    public class Request
    {
        public Request(string httpMethod, string path, int queryStringParameterCount, int headerCount)
        {
            HttpMethod = httpMethod;
            Path = path;
            QueryStringParameterCount = queryStringParameterCount;
            HeaderCount = headerCount;
        }

        public int HeaderCount { get; }
        public string HttpMethod { get; }
        public string Path { get; }
        public int QueryStringParameterCount { get; }
    }
}