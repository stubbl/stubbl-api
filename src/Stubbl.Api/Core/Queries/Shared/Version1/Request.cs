namespace Stubbl.Api.Core.Queries.Shared.Version1
{
   using System.Collections.Generic;

   public class Request
   {
      public Request(string httpMethod, string path, IReadOnlyCollection<QueryStringParameter> queryStringParameters,
         IReadOnlyCollection<BodyToken> bodyTokens, IReadOnlyCollection<Header> headers)
      {
         HttpMethod = httpMethod;
         Path = path;
         QueryStringParameters = queryStringParameters;
         BodyTokens = bodyTokens;
         Headers = headers;
      }

      public IReadOnlyCollection<BodyToken> BodyTokens { get; }
      public IReadOnlyCollection<Header> Headers { get; }
      public string HttpMethod { get; }
      public string Path { get; }
      public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; }
   }
}