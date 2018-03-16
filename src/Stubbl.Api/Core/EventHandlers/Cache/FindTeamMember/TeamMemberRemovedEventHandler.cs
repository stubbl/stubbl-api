﻿namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamMember
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using CodeContrib.Caching;
   using CodeContrib.EventHandlers;
   using Events.TeamMemberRemoved.Version1;

   public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public TeamMemberRemovedEventHandler(ICache cache, ICacheKey cacheKey)
      {
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindTeamMember
         (
            @event.TeamId,
            @event.MemberId
         ));

         return Task.CompletedTask;
      }
   }
}
