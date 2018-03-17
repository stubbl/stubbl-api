using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using NSubstitute;
using Stubbl.Api.Queries.ListTeamStubs.Version1;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
    public class WhenListingTeamStubs : IntegrationTest<IReadOnlyCollection<Stub>>
    {
        private static readonly Paging s_paging;

        static WhenListingTeamStubs()
        {
            s_paging = new Paging(1, 10, 1);
        }

        public WhenListingTeamStubs()
            : base(1, HttpStatusCode.OK, GenerateExpectedResponse(), GenerateExpectedHeaders())
        {
        }

        protected override HttpRequestMessage RequestMessage
        {
            get
            {
                QueryDispatcher.DispatchAsync(Arg.Any<ListTeamStubsQuery>(), Arg.Any<CancellationToken>())
                    .Returns(new ListTeamStubsProjection(ExpectedResponse, s_paging));

                return new HttpRequestMessage(HttpMethod.Get, $"/teams/{ObjectId.GenerateNewId()}/stubs/list");
            }
        }

        private static IHeaderDictionary GenerateExpectedHeaders()
        {
            return new HeaderDictionary
            {
                {"X-Paging-PageCount", s_paging.PageCount.ToString()},
                {"X-Paging-PageNumber", s_paging.PageNumber.ToString()},
                {"X-Paging-PageSize", s_paging.PageSize.ToString()},
                {"X-Paging-TotalCount", s_paging.TotalCount.ToString()}
            };
        }

        private static IReadOnlyCollection<Stub> GenerateExpectedResponse()
        {
            return new[]
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
            };
        }
    }
}