namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.QueryHandlers;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.ListTeamInvitations.Version1;
   using Queries.Shared.Version1;
   using Invitation = Data.Collections.Invitations.Invitation;

   public class ListTeamInvitationsQueryHandler : IQueryHandler<ListTeamInvitationsQuery, ListTeamInvitationsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public ListTeamInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<ListTeamInvitationsProjection> HandleAsync(ListTeamInvitationsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         return new ListTeamInvitationsProjection
         (
            await _invitationsCollection.Find(i => i.Team.Id == query.TeamId)
               .Project(i => new Queries.ListTeamInvitations.Version1.Invitation
               (
                  i.Id.ToString(),
                  i.Team.Id.ToString(),
                  new Role
                  (
                     i.Role.Id.ToString(),
                     i.Role.Name
                  ),
                  i.Status.ToInvitationStatus(),
                  i.EmailAddress
               ))
               .ToListAsync(cancellationToken)
         );
      }
   }
}