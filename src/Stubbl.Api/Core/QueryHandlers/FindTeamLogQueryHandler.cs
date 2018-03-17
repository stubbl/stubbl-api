namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.QueryHandlers;
   using Data.Collections.Logs;
   using Exceptions.LogNotFound.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Queries.FindLog.Version1;
   using Queries.Shared.Version1;

   public class FindTeamLogQueryHandler : IQueryHandler<FindTeamLogQuery, FindTeamLogProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Log> _logsCollection;

      public FindTeamLogQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Log> logsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _logsCollection = logsCollection;
      }

      public async Task<FindTeamLogProjection> HandleAsync(FindTeamLogQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
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