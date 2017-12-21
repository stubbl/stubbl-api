namespace Stubbl.Api.Tests.Integration.Controllers.Version1
{
   using System;
   using System.Net;
   using System.Net.Http;
   using System.Threading;
   using Core.Queries.FindTeamStub.Version1;
   using Core.Queries.Shared.Version1;
   using MongoDB.Bson;
   using NSubstitute;

   public class When_finding_a_team_stub : IntegrationTest<FindTeamStubProjection>
   {
      public When_finding_a_team_stub()
         : base(1, HttpStatusCode.OK, GenerateExpectedResponse())
      {
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            QueryDispatcher.DispatchAsync(Arg.Any<FindTeamStubQuery>(), Arg.Any<CancellationToken>())
               .Returns(ExpectedResponse);

            return new HttpRequestMessage(HttpMethod.Get,
               $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/find");
         }
      }

      private static FindTeamStubProjection GenerateExpectedResponse()
      {
         return new FindTeamStubProjection
         (
            ObjectId.GenerateNewId().ToString() /* stubId */,
            ObjectId.GenerateNewId().ToString() /* teamId */,
            Guid.NewGuid().ToString(),
            new Request
            (
               "/foo",
               "GET",
               null /* queryStringParameters */,
               null /* bodyTokens */,
               null /* headers */
            ),
            new Response
            (
               200,
               null /* body */,
               null /* headers */
            ),
            null /* tags */
         );
      }
   }
}