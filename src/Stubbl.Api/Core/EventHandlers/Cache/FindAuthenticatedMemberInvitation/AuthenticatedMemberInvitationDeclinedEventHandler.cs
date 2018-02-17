namespace Stubbl.Api.Core.EventHandlers.Cache.FindAuthenticatedMemberInvitation
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Caching;
   using Common.Caching;
   using Common.EventHandlers;
   using Events.AuthenticatedMemberInvitationDeclined.Version1;

   public class AuthenticatedMemberInvitationDeclinedEventHandler : IEventHandler<AuthenticatedMemberInvitationDeclinedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICache _cache;
      private readonly ICacheKey _cacheKey;

      public AuthenticatedMemberInvitationDeclinedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor, 
         ICache cache, ICacheKey cacheKey)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _cache = cache;
         _cacheKey = cacheKey;
      }

      public Task HandleAsync(AuthenticatedMemberInvitationDeclinedEvent @event, CancellationToken cancellationToken)
      {
         _cache.Remove(_cacheKey.FindAuthenticatedMemberInvitation
         (
            _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress,
            @event.InvitationId
         ));

         return Task.CompletedTask;
      }
   }
}
