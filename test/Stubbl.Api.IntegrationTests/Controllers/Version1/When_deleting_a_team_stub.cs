namespace Stubbl.Api.IntegrationTests.Controllers.Version1
{
   using System.Net;
   using System.Net.Http;
   using MongoDB.Bson;

   public class When_deleting_a_team_stub : IntegrationTest
   {
      public When_deleting_a_team_stub()
         : base(1, HttpStatusCode.NoContent)
      {
      }

      protected override HttpRequestMessage RequestMessage
         => new HttpRequestMessage(HttpMethod.Post,
            $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/delete");
   }
}