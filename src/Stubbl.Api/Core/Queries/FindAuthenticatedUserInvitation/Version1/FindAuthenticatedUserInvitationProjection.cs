namespace Stubbl.Api.Core.Queries.FindAuthenticatedUserInvitation.Version1
{
   using CodeContrib.Queries;

   public class FindAuthenticatedUserInvitationProjection : IProjection
   {
      public FindAuthenticatedUserInvitationProjection(string id, Team team, Role role)
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