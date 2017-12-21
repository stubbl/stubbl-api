﻿namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.QueryHandlers;
   using Data.Collections.Teams;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.TeamNotFound.Version1;
   using MongoDB.Driver;
   using Queries.FindTeam.Version1;

   public class FindTeamQueryHandler : IQueryHandler<FindTeamQuery, FindTeamProjection>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Team> _teamsCollection;

      public FindTeamQueryHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICache cache, ICacheKey cacheKey, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
         _teamsCollection = teamsCollection;
      }

      public async Task<FindTeamProjection> HandleAsync(FindTeamQuery query, CancellationToken cancellationToken)
      {
         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.All(t => t.Id != query.TeamId))
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               query.TeamId
            );
         }

         var team = await _cache.GetOrSetAsync
         (
            _cacheKey.FindTeam(query.TeamId), 
            async () => await _teamsCollection.Find(t => t.Id == query.TeamId)
               .Project(t => new FindTeamProjection
               (
                  t.Id.ToString(),
                  t.Name
               ))
               .SingleOrDefaultAsync(cancellationToken)
         );

         if (team == null)
         {
            throw new TeamNotFoundException
            (
               query.TeamId
            );
         }

         return team;
      }
   }
}