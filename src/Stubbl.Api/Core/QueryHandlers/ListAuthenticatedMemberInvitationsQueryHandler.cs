namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using MongoDB.Driver;
   using Queries.ListAuthenticatedMemberInvitations.Version1;
   using Queries.Shared.Version1;
   using Invitation = Data.Collections.Invitations.Invitation;

   public class ListAuthenticatedMemberInvitationsQueryHandler : IQueryHandler<ListAuthenticatedMemberInvitationsQuery, ListAuthenticatedMemberInvitationsProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public ListAuthenticatedMemberInvitationsQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<ListAuthenticatedMemberInvitationsProjection> HandleAsync(ListAuthenticatedMemberInvitationsQuery query, CancellationToken cancellationToken)
      {
         return new ListAuthenticatedMemberInvitationsProjection
         (
            await _invitationsCollection.Find(i => i.EmailAddress.ToLower() == _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress.ToLower())
               .Project(i => new Queries.ListAuthenticatedMemberInvitations.Version1.Invitation
               (
                  i.Id.ToString(),
                  new Team
                  (
                     i.Team.Id.ToString(),
                     i.Team.Name
                  ),
                  new Role
                  (
                     i.Role.Id.ToString(),
                     i.Role.Name
                  ),
                  i.Status.ToInvitationStatus()
               ))
               .ToListAsync(cancellationToken)
         );
      }
   }
}