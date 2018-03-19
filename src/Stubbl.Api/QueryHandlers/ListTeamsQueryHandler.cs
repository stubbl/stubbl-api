using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Authentication;
using Stubbl.Api.Queries.ListTeams.Version1;

namespace Stubbl.Api.QueryHandlers
{
    public class ListTeamsQueryHandler : IQueryHandler<ListTeamsQuery, ListTeamsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;

        public ListTeamsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
        }

        public Task<ListTeamsProjection> HandleAsync(ListTeamsQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ListTeamsProjection
            (
                _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => new Team
                    (
                        t.Id.ToString(),
                        t.Name
                    ))
                    .ToList()
            ));
        }
    }
}