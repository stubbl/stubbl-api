namespace Stubbl.Api.Core.Queries.FindTeamInvitation.Version1
{
   using Gunnsoft.Cqs.Queries;
   using Shared.Version1;

   public class FindTeamInvitationProjection : IProjection
   {
      public FindTeamInvitationProjection(string id, Role role, InvitationStatus status)
      {
         Id = id;
         Role = role;
         Status = status;
      }

      public string Id { get; }
      public Role Role { get; }
      public InvitationStatus Status { get; }
   }
}