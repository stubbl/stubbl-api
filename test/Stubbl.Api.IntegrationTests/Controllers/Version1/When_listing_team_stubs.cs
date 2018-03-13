namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
    using Core.Queries.ListTeamStubs.Version1;
    using Core.Queries.Shared.Version1;
    using Microsoft.AspNetCore.Http;
    using MongoDB.Bson;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    public class When_listing_team_stubs : IntegrationTest<IReadOnlyCollection<Stub>>
    {
        private static readonly Paging _paging;

        static When_listing_team_stubs()
        {
            _paging = new Paging(1, 10, 1);
        }

        public When_listing_team_stubs()
           : base(1, HttpStatusCode.OK, GenerateExpectedResponse(), GenerateExpectedHeaders())
        {
        }

        protected override HttpRequestMessage RequestMessage
        {
            get
            {
                QueryDispatcher.DispatchAsync(Arg.Any<ListTeamStubsQuery>(), Arg.Any<CancellationToken>())
                   .Returns(new ListTeamStubsProjection(ExpectedResponse, _paging));

                return new HttpRequestMessage(HttpMethod.Get, $"/teams/{ObjectId.GenerateNewId()}/stubs/list");
            }
        }

        private static IHeaderDictionary GenerateExpectedHeaders()
        {
            return new HeaderDictionary
            {
                { "X-Paging-PageCount", _paging.PageCount.ToString() },
                { "X-Paging-PageNumber", _paging.PageNumber.ToString() },
                { "X-Paging-PageSize", _paging.PageSize.ToString() },
                { "X-Paging-TotalCount", _paging.TotalCount.ToString() }
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