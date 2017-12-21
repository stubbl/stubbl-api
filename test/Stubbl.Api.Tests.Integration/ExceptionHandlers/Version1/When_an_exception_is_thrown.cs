namespace Stubbl.Api.Tests.Integration.ExceptionHandlers.Version1
{
   using System;
   using System.Net;
   using System.Net.Http;
   using System.Threading;
   using Core.Queries.Canary.Version1;
   using NSubstitute;
   using NSubstitute.ExceptionExtensions;
   using NUnit.Framework;

   [TestFixtureSource(typeof(ExceptionHandlerTestFixtureData))]
   public class When_an_exception_is_thrown : IntegrationTest
   {
      public When_an_exception_is_thrown(Exception exception, HttpStatusCode expectedStatusCode, object expectedResponse)
         : base(1, expectedStatusCode, expectedResponse)
      {
         QueryDispatcher.DispatchAsync(Arg.Any<CanaryQuery>(), Arg.Any<CancellationToken>())
            .Throws(exception);
      }

      protected override HttpRequestMessage RequestMessage => new HttpRequestMessage(HttpMethod.Get, "/canary");
   }
}