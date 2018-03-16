namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.QueryHandlers;
   using Core.Authentication;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Data.Collections.Logs;
   using MongoDB.Driver;
   using Queries.CountTeamLogs.Version1;

   public class CountTeamLogsQueryHandler : IQueryHandler<CountTeamLogsQuery, CountTeamLogsProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Log> _logsCollection;

      public CountTeamLogsQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Log> logsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _logsCollection = logsCollection;
      }

      public async Task<CountTeamLogsProjection> HandleAsync(CountTeamLogsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var totalCount = await _cache.GetOrSetAsync
         (
            _cacheKey.CountTeamLogs(query.TeamId),
            async () => await _logsCollection.CountAsync(l => l.TeamId == query.TeamId)
         );

         return new CountTeamLogsProjection
         (
            totalCount
         );
      }
   }
}
