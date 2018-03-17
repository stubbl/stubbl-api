namespace Stubbl.Api.Core.EventHandlers.Cache.FindTeamInvitation
{
   using System.Threading;
   using System.Threading.Tasks;
   using Caching;
   using Gunnsoft.Common.Caching;
   using Gunnsoft.Cqs.EventHandlers;
   using Data.Collections.Invitations;
   using Events.AuthenticatedUserInvitationAccepted.Version1;
   using MongoDB.Driver;

   public class AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
   {
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public AuthenticatedUserInvitationAcceptedEventHandler(ICache cache, ICacheKey cacheKey, 
         IMongoCollection<Invitation> invitationsCollection)
      {
         _cache = cache;
         _cacheKey = cacheKey;
         _invitationsCollection = invitationsCollection;
      }

      public async Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event, CancellationToken cancellationToken)
      {
         var teamId = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
            .Project(i => i.Team.Id)
            .SingleAsync(cancellationToken);

         _cache.Remove(_cacheKey.FindTeamInvitation
         (
            teamId,
            @event.InvitationId
         ));
      }
   }
}
