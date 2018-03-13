namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
   using System.Net;
   using System.Net.Http;
   using System.Text;
   using System.Threading;
   using Core.Commands.CloneTeamStub.Version1;
   using Core.Events.Shared.Version1;
   using Core.Events.TeamStubCloned.Version1;
   using FluentAssertions;
   using Models.CloneTeamStub.Version1;
   using MongoDB.Bson;
   using NSubstitute;
   using NUnit.Framework;

   public class When_cloning_a_team_stub : IntegrationTest<CloneTeamStubResponse>
   {
      private readonly ObjectId _teamId;

      public When_cloning_a_team_stub()
         : base(1, HttpStatusCode.Created, new CloneTeamStubResponse(ObjectId.GenerateNewId().ToString()))
      {
         _teamId = ObjectId.GenerateNewId();
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            const string body = @"{
   ""name"": ""Foo2""
}";

            var @event = new TeamStubClonedEvent
            (
               ObjectId.Parse(ExpectedResponse.StubId),
               _teamId,
               "Foo",
               new Request
               (
                  "GET",
                  "/foo",
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

            CommandDispatcher.DispatchAsync(Arg.Any<CloneTeamStubCommand>(), Arg.Any<CancellationToken>())
               .Returns(@event);

            return new HttpRequestMessage(HttpMethod.Post, $"/teams/{@event.TeamId}/stubs/{@event.StubId}/clone")
            {
               Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
         }
      }

      [Test]
      public void Then_the_location_header_should_be_a_link_to_the_resource()
      {
         ResponseMessage.Headers.Location.Should().Be($"http://localhost/teams/{_teamId}/stubs/{ExpectedResponse.StubId}/find");
      }
   }
}