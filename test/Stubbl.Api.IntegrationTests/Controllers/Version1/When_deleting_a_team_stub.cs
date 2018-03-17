using System.Net;
using System.Net.Http;
using MongoDB.Bson;

namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
    public class WhenDeletingATeamStub : IntegrationTest
    {
        public WhenDeletingATeamStub()
            : base(1, HttpStatusCode.NoContent)
        {
        }

        protected override HttpRequestMessage RequestMessage
            => new HttpRequestMessage(HttpMethod.Post,
                $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/delete");
    }
}