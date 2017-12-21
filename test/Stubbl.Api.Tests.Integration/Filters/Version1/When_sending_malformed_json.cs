namespace Stubbl.Api.Tests.Integration.Filters.Version1
{
   using System.Net;
   using System.Net.Http;
   using System.Text;
   using NUnit.Framework;

   [TestFixtureSource(typeof(MalformedJsonTextFixtureData))]
   public class When_sending_malformed_json : IntegrationTest
   {
      private readonly HttpMethod _httpMethod;
      private readonly string _path;

      public When_sending_malformed_json(HttpMethod httpMethod, string path)
         : base(1, HttpStatusCode.BadRequest, null)
      {
         _httpMethod = httpMethod;
         _path = path;
      }

      protected override HttpRequestMessage RequestMessage
      {
         get
         {
            const string body = "Malformed JSON";

            return new HttpRequestMessage(_httpMethod, _path)
            {
               Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
         }
      }
   }
}
