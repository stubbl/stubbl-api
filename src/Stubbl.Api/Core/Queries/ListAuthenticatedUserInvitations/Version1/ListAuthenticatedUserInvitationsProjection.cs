namespace Stubbl.Api.Core.Queries.ListAuthenticatedUserInvitations.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Queries;

   public class ListAuthenticatedUserInvitationsProjection : IProjection
   {
      public ListAuthenticatedUserInvitationsProjection(IReadOnlyCollection<Invitation> invitations)
      {
         Invitations = invitations;
      }

      public IReadOnlyCollection<Invitation> Invitations { get; }
   }
}