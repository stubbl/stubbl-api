namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.QueryHandlers;
   using Core.Authentication;
   using Data.Collections.Invitations;
   using MongoDB.Driver;
   using Queries.CountAuthenticatedMemberInvitations.Version1;

   public class CountAuthenticatedMemberInvitationsQueryHandler : IQueryHandler<CountAuthenticatedMemberInvitationsQuery, CountAuthenticatedMemberInvitationsProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public CountAuthenticatedMemberInvitationsQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<CountAuthenticatedMemberInvitationsProjection> HandleAsync(CountAuthenticatedMemberInvitationsQuery query, CancellationToken cancellationToken)
      {
         var totalCount = await _invitationsCollection.CountAsync(i => i.EmailAddress.ToLower() == _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress.ToLower());

         return new CountAuthenticatedMemberInvitationsProjection
         (
            totalCount
         );
      }
   }
}
