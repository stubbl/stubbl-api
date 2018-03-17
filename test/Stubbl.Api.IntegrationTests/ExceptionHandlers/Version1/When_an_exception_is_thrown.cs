using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Stubbl.Api.Queries.Canary.Version1;

namespace Stubbl.Api.IntegrationTests.ExceptionHandlers.Version1
{
    [TestFixtureSource(typeof(ExceptionHandlerTestFixtureData))]
    public class WhenAnExceptionIsThrown : IntegrationTest
    {
        public WhenAnExceptionIsThrown(Exception exception, HttpStatusCode expectedStatusCode, object expectedResponse)
            : base(1, expectedStatusCode, expectedResponse)
        {
            QueryDispatcher.DispatchAsync(Arg.Any<CanaryQuery>(), Arg.Any<CancellationToken>())
                .Throws(exception);
        }

        protected override HttpRequestMessage RequestMessage => new HttpRequestMessage(HttpMethod.Get, "/canary");
    }
}