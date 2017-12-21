namespace Stubbl.Api.Models.Shared.Version1
{
   using System.Collections.Generic;

   public class Request
   {
      public IReadOnlyCollection<BodyToken> BodyTokens { get; set; }
      public IReadOnlyCollection<Header> Headers { get; set; }
      public string HttpMethod { get; set; }
      public string Path { get; set; }
      public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; set; }
   }
}