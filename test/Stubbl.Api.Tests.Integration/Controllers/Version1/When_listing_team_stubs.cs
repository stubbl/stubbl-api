namespace Stubbl.Api.Tests.Integration.Controllers.Version1
{
   using System.Net;
   using System.Net.Http;
   using System.Threading;
   using Core.Queries.ListTeamStubs.Version1;
   using Core.Queries.Shared.Version1;
   using MongoDB.Bson;
   using NSubstitute;

   public class When_listing_team_stubs : IntegrationTest<ListTeamStubsProjection>
   {
      public When_listing_team_stubs()
         : base(1, HttpStatusCode.OK, GenerateExpectedResponse())
      {
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            QueryDispatcher.DispatchAsync(Arg.Any<ListTeamStubsQuery>(), Arg.Any<CancellationToken>())
               .Returns(ExpectedResponse);

            return new HttpRequestMessage(HttpMethod.Get, $"/teams/{ObjectId.GenerateNewId()}/stubs/list");
         }
      }

      private static ListTeamStubsProjection GenerateExpectedResponse()
      {
         return new ListTeamStubsProjection
         (
            new[]
            {
               new Stub
               (
                  ObjectId.GenerateNewId().ToString() /* stubId */,
                  ObjectId.GenerateNewId().ToString() /* teamId */,
                  "foo",
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
                  new[]
                  {
                     "a",
                     "b",
                     "c"
                  })
            },
            new Paging
            (
               1,
               10,
               1
            )
         );
      }
   }
}