namespace Stubbl.Api.Core.Data.Collections.Invitations
{
   using MongoDB.Bson;

   public class Team
   {
      public ObjectId Id { get; set; }
      public string Name { get; set; }
   }
}