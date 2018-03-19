using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Queries.CountAuthenticatedUserInvitations.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class CountAuthenticatedUserInvitationsQueryHandler : IQueryHandler<CountAuthenticatedUserInvitationsQuery,
        CountAuthenticatedUserInvitationsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Invitation> _invitationsCollection;

        public CountAuthenticatedUserInvitationsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Invitation> invitationsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _invitationsCollection = invitationsCollection;
        }

        public async Task<CountAuthenticatedUserInvitationsProjection> HandleAsync(
            CountAuthenticatedUserInvitationsQuery query, CancellationToken cancellationToken)
        {
            var totalCount = await _invitationsCollection.CountAsync(i =>
                i.EmailAddress.ToLower() == _authenticatedUserAccessor.AuthenticatedUser.EmailAddress.ToLower());

            return new CountAuthenticatedUserInvitationsProjection
            (
                totalCount
            );
        }
    }
}