namespace Stubbl.Api.Core.Data.Collections.Members
{
   using System.Collections.Generic;
   using MongoDB.Bson;

   public class Member
   {
      public Member()
      {
         Teams = new Team[0];
      }

      public string EmailAddress { get; set; }
      public ObjectId Id { get; set; }
      public string IdentityId { get; set; }
      public string Name { get; set; }
      public IReadOnlyCollection<Team> Teams { get; set; }
   }
}