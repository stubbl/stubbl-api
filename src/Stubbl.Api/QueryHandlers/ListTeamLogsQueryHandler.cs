using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Queries;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Stubbl.Api.Queries.ListTeamLogs.Version1;
using Stubbl.Api.Queries.Shared.Version1;
using Log = Stubbl.Api.Data.Collections.Logs.Log;

namespace Stubbl.Api.QueryHandlers
{
    public class ListTeamLogsQueryHandler : IQueryHandler<ListTeamLogsQuery, ListTeamLogsProjection>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Log> _logsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public ListTeamLogsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Log> logsCollection, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _logsCollection = logsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task<ListTeamLogsProjection> HandleAsync(ListTeamLogsQuery query,
            CancellationToken cancellationToken)
        {
            if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
            {
                throw new MemberNotAddedToTeamException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    query.TeamId
                );
            }

            var filter = Builders<Log>.Filter.Where(l => l.TeamId == query.TeamId);

            if (query.StubIds != null)
            {
                foreach (var stubId in query.StubIds)
                {
                    filter = Builders<Log>.Filter.And(filter,
                        Builders<Log>.Filter.Where(l => l.StubIds.Contains(stubId)));
                }
            }

            var sort = Builders<Log>.Sort.Descending(l => l.Id);

            var totalCount = await _logsCollection.Find(filter).CountAsync(cancellationToken);
            var logs = await _logsCollection.Find(filter)
                .Sort(sort)
                .Project(l => new Queries.ListTeamLogs.Version1.Log
                (
                    l.Id.ToString(),
                    l.TeamId.ToString(),
                    l.StubIds.Select(si => si.ToString()).ToList(),
                    new RequestLog
                    (
                        l.Request.Path,
                        l.Request.HttpMethod,
                        l.Request.QueryStringParameters.Select(qcc => new QueryStringParameter(qcc.Key, qcc.Value))
                            .ToList(),
                        l.Request.Body,
                        l.Request.Headers.Select(qcc => new Header(qcc.Key, qcc.Value)).ToList()
                    ),
                    new ResponseLog
                    (
                        l.Response.HttpStatusCode,
                        l.Response.Body,
                        l.Response.Headers.Select(qcc => new Header(qcc.Key, qcc.Value)).ToList()
                    ),
                    l.Id.CreationTime
                ))
                .ToListAsync(cancellationToken);

            return new ListTeamLogsProjection
            (
                logs,
                new Paging
                (
                    query.PageNumber,
                    query.PageSize,
                    totalCount
                )
            );
        }
    }
}