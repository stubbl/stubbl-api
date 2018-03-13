namespace Stubbl.Api.IntegrationTests
{
    using Microsoft.AspNetCore.Http;
    using System.Net;

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