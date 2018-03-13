namespace Stubbl.Api.Core.Queries.ListAuthenticatedUserInvitations.Version1
{
   using Shared.Version1;

   public class Invitation
   {
      public Invitation(string id, Team team, Role role, InvitationStatus status)
      {
         Id = id;
         Team = team;
         Role = role;
         Status = status;
      }

      public string Id { get; }
      public Role Role { get; }
      public InvitationStatus Status { get; }
      public Team Team { get; }
   }
}