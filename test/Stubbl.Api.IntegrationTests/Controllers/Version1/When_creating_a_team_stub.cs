using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using FluentAssertions;
using MongoDB.Bson;
using NSubstitute;
using NUnit.Framework;
using Stubbl.Api.Commands.CreateTeamStub.Version1;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamStubCreated.Version1;
using Stubbl.Api.Models.CreateTeamStub.Version1;

namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
    public class WhenCreatingATeamStub : IntegrationTest<CreateTeamStubResponse>
    {
        private readonly ObjectId _teamId;

        public WhenCreatingATeamStub()
            : base(1, HttpStatusCode.Created, new CreateTeamStubResponse(ObjectId.GenerateNewId().ToString()))
        {
            _teamId = ObjectId.GenerateNewId();
        }

        protected override HttpRequestMessage RequestMessage
        {
            get
            {
                var @event = new TeamStubCreatedEvent
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

                var body = $@"{{
   ""name"": ""{@event.Name}"",
	""request"": {{
      ""httpMethod"": ""{@event.Request.HttpMethod}"",
	   ""path"": ""{@event.Request.Path}""
   }},
   ""response"": {{
      ""httpStatusCode"": {@event.Response.HttpStatusCode}
   }},
	""teamId"": ""{@event.TeamId}""
}}";

                CommandDispatcher.DispatchAsync(Arg.Any<CreateTeamStubCommand>(), Arg.Any<CancellationToken>())
                    .Returns(@event);

                return new HttpRequestMessage(HttpMethod.Post, $"/teams/{@event.TeamId}/stubs/create")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };
            }
        }

        [Test]
        public void Then_the_location_header_should_be_a_link_to_the_resource()
        {
            ResponseMessage.Headers.Location.Should()
                .Be($"http://localhost/teams/{_teamId}/stubs/{ExpectedResponse.StubId}/find");
        }
    }
}