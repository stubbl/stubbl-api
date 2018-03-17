namespace Stubbl.Api.Core.Queries.FindTeamMember.Version1
{
   using Gunnsoft.Cqs.Queries;

   public class FindTeamMemberProjection : IProjection
   {
      public FindTeamMemberProjection(string id, string name, string emailAddress, Role role)
      {
         Id = id;
         Name = name;
         EmailAddress = emailAddress;
         Role = role;
      }

      public string Id { get; }
      public string EmailAddress { get; }
      public string Name { get; }
      public Role Role { get; }
   }
}