using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;

namespace Stubbl.Api.IntegrationTests.Filters.Version1
{
    [TestFixtureSource(typeof(MalformedJsonTextFixtureData))]
    public class WhenSendingMalformedJson : IntegrationTest
    {
        private readonly HttpMethod _httpMethod;
        private readonly string _path;

        public WhenSendingMalformedJson(HttpMethod httpMethod, string path)
            : base(1, HttpStatusCode.BadRequest, null)
        {
            _httpMethod = httpMethod;
            _path = path;
        }

        protected override HttpRequestMessage RequestMessage
        {
            get
            {
                const string body = "Malformed JSON";

                return new HttpRequestMessage(_httpMethod, _path)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}