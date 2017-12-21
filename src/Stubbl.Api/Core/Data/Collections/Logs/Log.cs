namespace Stubbl.Api.Core.Data.Collections.Logs
{
   using System.Collections.Generic;
   using MongoDB.Bson;

   public class Log
   {
      public Log()
      {
         StubIds = new ObjectId[0];
      }

      public ObjectId Id { get; set; }
      public Request Request { get; set; }
      public Response Response { get; set; }
      public IReadOnlyCollection<ObjectId> StubIds { get; set; }
      public ObjectId TeamId { get; set; }
   }
}