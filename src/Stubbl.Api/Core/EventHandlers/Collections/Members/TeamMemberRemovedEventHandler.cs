namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamMemberRemoved.Version1;
   using MongoDB.Driver;

   public class TeamMemberRemovedEventHandler : IEventHandler<TeamMemberRemovedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamMemberRemovedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Member> membersCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamMemberRemovedEvent @event, CancellationToken cancellationToken)
      {
         var filter = Builders<Member>.Filter.Where(m => m.Id == @event.MemberId);
         var update = Builders<Member>.Update.PullFilter(t => t.Teams, m => m.Id == @event.TeamId);

         await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
      }
   }
}
