namespace Stubbl.Api.Core.Data.Collections.Members
{
   using MongoDB.Bson;

   public class Team
   {
      public ObjectId Id { get; set; }
      public string Name { get; set; }
      public Role Role { get; set; }
   }
}