namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.QueryHandlers;
   using MongoDB.Driver;
   using Queries.ListAuthenticatedUserInvitations.Version1;
   using Queries.Shared.Version1;
   using Invitation = Data.Collections.Invitations.Invitation;

   public class ListAuthenticatedUserInvitationsQueryHandler : IQueryHandler<ListAuthenticatedUserInvitationsQuery, ListAuthenticatedUserInvitationsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public ListAuthenticatedUserInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<ListAuthenticatedUserInvitationsProjection> HandleAsync(ListAuthenticatedUserInvitationsQuery query, CancellationToken cancellationToken)
      {
         return new ListAuthenticatedUserInvitationsProjection
         (
            await _invitationsCollection.Find(i => i.EmailAddress.ToLower() == _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower())
               .Project(i => new Queries.ListAuthenticatedUserInvitations.Version1.Invitation
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