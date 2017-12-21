namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.QueryHandlers;
   using Core.Authentication;
   using Core.Exceptions.MemberNotAddedToTeam.Version1;
   using Data.Collections.Logs;
   using MongoDB.Driver;
   using Queries.CountTeamLogs.Version1;

   public class CountTeamLogsQueryHandler : IQueryHandler<CountTeamLogsQuery, CountTeamLogsProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Log> _logsCollection;

      public CountTeamLogsQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Log> logsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _logsCollection = logsCollection;
      }

      public async Task<CountTeamLogsProjection> HandleAsync(CountTeamLogsQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
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
