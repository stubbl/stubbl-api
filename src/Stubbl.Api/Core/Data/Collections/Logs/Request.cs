namespace Stubbl.Api.Core.Data.Collections.Logs
{
   using System.Collections.Generic;
   using Shared;

   public class Request
   {
      public Request()
      {
         Headers = new Header[0];
         QueryStringParameters = new QueryStringParameter[0];
      }

      public string Body { get; set; }
      public IReadOnlyCollection<Header> Headers { get; set; }
      public string HttpMethod { get; set; }
      public string Path { get; set; }
      public IReadOnlyCollection<QueryStringParameter> QueryStringParameters { get; set; }
   }
}