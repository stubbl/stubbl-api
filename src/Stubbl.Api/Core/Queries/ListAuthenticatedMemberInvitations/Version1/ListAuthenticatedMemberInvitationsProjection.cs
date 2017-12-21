namespace Stubbl.Api.Core.Queries.ListAuthenticatedMemberInvitations.Version1
{
   using System.Collections.Generic;
   using Common.Queries;

   public class ListAuthenticatedMemberInvitationsProjection : IProjection
   {
      public ListAuthenticatedMemberInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
      {
         Invitations = invitations;
      }

      public IReadOnlyCollection<Invitation> Invitations { get; }
   }
}