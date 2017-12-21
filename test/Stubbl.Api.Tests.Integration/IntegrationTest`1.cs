namespace Stubbl.Api.Tests.Integration
{
   using System.Net;

   public abstract class IntegrationTest<TResponse> : IntegrationTest
      where TResponse : class
   {
      protected IntegrationTest(int versionNumber, HttpStatusCode expectedStatusCode, TResponse expectedResponse)
         : base(versionNumber, expectedStatusCode, expectedResponse)
      {
         ExpectedResponse = expectedResponse;
      }

      protected TResponse ExpectedResponse { get; }
   }
}