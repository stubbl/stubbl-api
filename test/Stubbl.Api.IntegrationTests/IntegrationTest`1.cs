using System.Net;
using Microsoft.AspNetCore.Http;

namespace Stubbl.Api.IntegrationTests
{
    public abstract class IntegrationTest<TResponse> : IntegrationTest
        where TResponse : class
    {
        protected IntegrationTest(int versionNumber, HttpStatusCode expectedStatusCode, TResponse expectedResponse,
            IHeaderDictionary expectedHeaders = null)
            : base(versionNumber, expectedStatusCode, expectedResponse, expectedHeaders)
        {
            ExpectedResponse = expectedResponse;
        }

        protected TResponse ExpectedResponse { get; }
    }
}