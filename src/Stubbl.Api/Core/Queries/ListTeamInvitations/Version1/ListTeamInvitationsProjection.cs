namespace Stubbl.Api.Core.Queries.ListTeamInvitations.Version1
{
   using System.Collections.Generic;
   using CodeContrib.Queries;

   public class ListTeamInvitationsProjection : IProjection
   {
      public ListTeamInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
      {
         Invitations = invitations;
      }

      public IReadOnlyCollection<Invitation> Invitations { get; }
   }
}