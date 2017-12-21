namespace Stubbl.Api.Core.Data.Collections.Stubs
{
   using System.Collections.Generic;
   using Shared;

   public class Request
   {
      public Request()
      {
         BodyTokens = new BodyToken[0];
         Headers = new Header[0];
         QueryStringParameters = new QueryStringParameter[0];
      }

      public IReadOnlyCollection<BodyToken> BodyTokens { get; set; }
      public IReadOnlyCollection<Header> Headers { get; set; }
      public string HttpMethod { get; set; }
      public string Path { get; set; }
      public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; set; }
   }
}