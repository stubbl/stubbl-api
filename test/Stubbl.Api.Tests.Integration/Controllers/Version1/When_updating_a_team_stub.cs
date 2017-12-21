namespace Stubbl.Api.Tests.Integration.Controllers.Version1
{
   using System.Net;
   using System.Net.Http;
   using System.Text;
   using MongoDB.Bson;

   public class When_updating_a_team_stub : IntegrationTest
   {
      public When_updating_a_team_stub()
         : base(1, HttpStatusCode.NoContent)
      {
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            const string body = @"{
   ""name"": ""Foo"",
	""request"": {
      ""httpMethod"": ""GET"",
      ""path"": ""/foo""
   },
   ""response"": {
      ""httpStatusCode"": 200
   }
}";

            return new HttpRequestMessage(HttpMethod.Post,
               $"/teams/{ObjectId.GenerateNewId()}/stubs/{ObjectId.GenerateNewId()}/update")
            {
               Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
         }
      }
   }
}