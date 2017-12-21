namespace Stubbl.Api.Core.Queries.ListAuthenticatedMemberInvitations.Version1
{
   public class Team
   {
      public Team(string id, string name)
      {
         Id = id;
         Name = name;
      }

      public string Id { get; }
      public string Name { get; }
   }
}