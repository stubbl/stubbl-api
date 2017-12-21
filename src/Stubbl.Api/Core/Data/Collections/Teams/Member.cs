namespace Stubbl.Api.Core.Data.Collections.Teams
{
   using MongoDB.Bson;

   public class Member
   {
      public string EmailAddress { get; set; }
      public ObjectId Id { get; set; }
      public string Name { get; set; }
      public Role Role { get; set; }
   }
}