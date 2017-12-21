namespace Stubbl.Api.Core.Data.Collections.Logs
{
   using System.Collections.Generic;
   using Shared;

   public class Response
   {
      public Response()
      {
         Headers = new Header[0];
      }

      public string Body { get; set; }
      public IReadOnlyCollection<Header> Headers { get; set; }
      public int HttpStatusCode { get; set; }
   }
}