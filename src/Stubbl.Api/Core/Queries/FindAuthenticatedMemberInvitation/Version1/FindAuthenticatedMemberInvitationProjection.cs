namespace Stubbl.Api.Core.Queries.FindAuthenticatedMemberInvitation.Version1
{
   using Common.Queries;

   public class FindAuthenticatedMemberInvitationProjection : IProjection
   {
      public FindAuthenticatedMemberInvitationProjection(string id, Team team, Role role)
      {
         Id = id;
         Team = team;
         Role = role;
      }

      public string Id { get; }
      public Role Role { get; }
      public Team Team { get; }
   }
}