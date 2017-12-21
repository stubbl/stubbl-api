namespace Stubbl.Api.Models.Shared.Version1
{
   using System.Collections.Generic;

   public class Response
   {
      public string Body { get; set; }
      public IReadOnlyCollection<Header> Headers { get; set; }
      public int? HttpStatusCode { get; set; }
   }
}