﻿namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeam
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Data.Collections.Invitations;
   using Events.AuthenticatedMemberInvitationAccepted.Version1;
   using MongoDB.Driver;

   public class AuthenticatedMemberInvitationAcceptedEventHandler : IEventHandler<AuthenticatedMemberInvitationAcceptedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public AuthenticatedMemberInvitationAcceptedEventHandler(ICache cache, ICacheKey cacheKey, 
         IMongoCollection<Invitation> invitationsCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task HandleAsync(AuthenticatedMemberInvitationAcceptedEvent @event, CancellationToken cancellationToken)
      {
         var teamId = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
            .Project(i => i.Team.Id)
            .SingleAsync(cancellationToken);

         _cache.Remove(_cacheKey.FindTeam
         (
            teamId
         ));
      }
   }
}
