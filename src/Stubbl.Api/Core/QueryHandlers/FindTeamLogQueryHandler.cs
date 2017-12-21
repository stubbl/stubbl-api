namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.QueryHandlers;
   using Data.Collections.Logs;
   using Exceptions.LogNotFound.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.FindLog.Version1;
   using Queries.Shared.Version1;

   public class FindTeamLogQueryHandler : IQueryHandler<FindTeamLogQuery, FindTeamLogProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Log> _logsCollection;

      public FindTeamLogQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Log> logsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _logsCollection = logsCollection;
      }

      public async Task<FindTeamLogProjection> HandleAsync(FindTeamLogQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               query.TeamId
            );
         }

         var log = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeamLog(query.TeamId, query.LogId), 
            async () => await _logsCollection.Find(l => l.TeamId == query.TeamId && l.Id == query.LogId)
               .Project(l => new FindTeamLogProjection
               (
                  l.Id.ToString(),
                  l.TeamId.ToString(),
                  l.StubIds.Select(sId => sId.ToString()).ToList(),
                  new RequestLog
                  (
                     l.Request.HttpMethod,
                     l.Request.Path,
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
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (log == null)
         {
            throw new LogNotFoundException
            (
               query.LogId,
               query.TeamId
            );
         }

         return log;
      }
   }
}