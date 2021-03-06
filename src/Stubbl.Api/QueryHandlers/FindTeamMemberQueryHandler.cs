using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Common.Caching;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Exceptions.MemberNotFound.Version1;
using Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1;
using Stubbl.Api.Queries.FindTeamMember.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Role = Stubbl.Api.Queries.FindTeamMember.Version1.Role;

namespace Stubbl.Api.QueryHandlers
{
    public class FindTeamMemberQueryHandler : IQueryHandler<FindTeamMemberQuery, FindTeamMemberProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly IMongoCollection<User> _usersCollection;

        public FindTeamMemberQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICache cache, ICacheKey cacheKey, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _cache = cache;
            _cacheKey = cacheKey;
            _usersCollection = usersCollection;
        }

        public async Task<FindTeamMemberProjection> HandleAsync(FindTeamMemberQuery query,
            CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new UserNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            var member = await _cache.GetOrSetAsync
            (
                _cacheKey.FindTeamMember(query.TeamId, query.MemberId),
                async () => await _usersCollection
                    .Find(m => m.Teams.Any(t => t.Id == query.TeamId) && m.Id == query.MemberId)
                    .Project(m => new FindTeamMemberProjection
                    (
                        m.Id.ToString(),
                        m.Name,
                        m.EmailAddress,
                        new Role
                        (
                            m.Teams.Single(t => t.Id == query.TeamId).Role.Id.ToString(),
                            m.Teams.Single(t => t.Id == query.TeamId).Role.Name,
                            m.Teams.Single(t => t.Id == query.TeamId).Role.Permissions
                                .Select(p => p.ToQueryPermission()).Where(p => p != null).ToList()
                        )
                    ))
                    .SingleOrDefaultAsync(cancellationToken)
            );

            if (member == null)
            {
                throw new MemberNotFoundException
                (
                    query.MemberId,
                    query.TeamId
                );
            }

            return member;
        }
    }
}