namespace Stubbl.Api.Core.Queries.ListTeamInvitations.Version1
{
   public class Role
   {
      public Role(string id, string name)
      {
         Id = id;
         Name = name;
      }

      public string Id { get; }
      public string Name { get; }
   }
}