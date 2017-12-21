namespace Stubbl.Api.Core.Queries.FindAuthenticatedMember.Version1
{
   public class Team
   {
      public Team(string id, string name, Role role)
      {
         Id = id;
         Name = name;
         Role = role;
      }

      public string Id { get; }
      public string Name { get; }
      public Role Role { get; }
   }
}
