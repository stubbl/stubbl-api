namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.QueryHandlers;
   using Data.Collections.Stubs;
   using Data.Collections.Teams;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.StubNotFound.Version1;
   using MongoDB.Driver;
   using Queries.FindTeamStub.Version1;
   using Queries.Shared.Version1;
   using BodyToken = Queries.Shared.Version1.BodyToken;
   using QueryStringParamter = Queries.Shared.Version1.QueryStringParameter;
   using Request = Queries.Shared.Version1.Request;
   using Response = Queries.Shared.Version1.Response;

   public class FindTeamStubQueryHandler : IQueryHandler<FindTeamStubQuery, FindTeamStubProjection>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Stub> _stubsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public FindTeamStubQueryHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICache cache, ICacheKey cacheKey, 
         IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _stubsCollection = stubsCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<FindTeamStubProjection> HandleAsync(FindTeamStubQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedUserAccessor.AuthenticatedUser.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               query.TeamId
            );
         }

         var stub = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeamStub(query.TeamId, query.StubId), 
            async () => await _stubsCollection.Find(s => s.TeamId == query.TeamId && s.Id == query.StubId)
               .Project(s => new FindTeamStubProjection
               (
                  s.Id.ToString(),
                  s.TeamId.ToString(),
                  s.Name,
                  new Request
                  (
                     s.Request.HttpMethod,
                     s.Request.Path,
                     s.Request.QueryStringParameters.Select(qcc => new QueryStringParamter(qcc.Key, qcc.Value)).ToList(),
                     s.Request.BodyTokens.Select(bt => new BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
                     s.Request.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
                  ),
                  new Response
                  (
                     s.Response.HttpStatusCode,
                     s.Response.Body,
                     s.Response.Headers.Select(h => new Header(h.Key, h.Value)).ToList()
                  ),
                  s.Tags
               ))
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (stub == null)
         {
            throw new StubNotFoundException
            (
               query.StubId,
               query.TeamId
            );
         }

         return stub;
      }
   }
}