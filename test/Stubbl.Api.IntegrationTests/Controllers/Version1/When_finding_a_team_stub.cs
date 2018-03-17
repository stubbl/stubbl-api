using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using MongoDB.Bson;
using NSubstitute;
using Stubbl.Api.Queries.FindTeamStub.Version1;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
    public class WhenFindingATeamStub : IntegrationTest<FindTeamStubProjection>
    {
        public WhenFindingATeamStub()
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