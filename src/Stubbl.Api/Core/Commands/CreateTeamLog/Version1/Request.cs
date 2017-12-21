namespace Stubbl.Api.Core.Commands.CreateTeamLog.Version1
{
   using System.Collections.Generic;
   using Shared.Version1;

   public class Request
   {
      public Request(string httpMethod, string path, IReadOnlyCollection<QueryStringParameter> queryStringParameters,
         string body, IReadOnlyCollection<Header> headers)
      {
         HttpMethod = httpMethod;
         Path = path;
         QueryStringParameters = queryStringParameters;
         Body = body;
         Headers = headers;
      }

      public string Body { get; }
      public IReadOnlyCollection<Header> Headers { get; }
      public string HttpMethod { get; }
      public string Path { get; }
      public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; }
   }
}