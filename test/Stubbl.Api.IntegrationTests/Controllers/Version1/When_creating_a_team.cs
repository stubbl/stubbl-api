namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
   using System.Net;
   using System.Net.Http;
   using System.Text;
   using System.Threading;
   using Core.Commands.CreateTeam.Version1;
   using Core.Events.TeamCreated.Version1;
   using FluentAssertions;
   using MongoDB.Bson;
   using Models.CreateTeam.Version1;
   using NSubstitute;
   using NUnit.Framework;

   public class When_creating_a_team : IntegrationTest<CreateTeamResponse>
   {
      public When_creating_a_team()
         : base(1, HttpStatusCode.Created, new CreateTeamResponse(ObjectId.GenerateNewId().ToString()))
      {
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            var @event = new TeamCreatedEvent
            (
               ObjectId.Parse(ExpectedResponse.TeamId),
               "Test",
               null, 
               null
            );

            var body = $@"{{
   ""name"": ""{@event.Name}"",
}}";

            CommandDispatcher.DispatchAsync(Arg.Any<CreateTeamCommand>(), Arg.Any<CancellationToken>())
               .Returns(@event);

            return new HttpRequestMessage(HttpMethod.Post, $"/teams/create")
            {
               Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
         }
      }

      [Test]
      public void Then_the_location_header_should_be_a_link_to_the_resource()
      {
         ResponseMessage.Headers.Location.Should().Be($"http://localhost/teams/{ExpectedResponse.TeamId}/find");
      }
   }
}