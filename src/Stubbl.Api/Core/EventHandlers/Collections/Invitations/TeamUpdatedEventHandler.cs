namespace Stubbl.Api.Core.EventHandlers.Collections.Invitations
{
   using System.Threading;
   using System.Threading.Tasks;
   using CodeContrib.EventHandlers;
   using Data.Collections.Invitations;
   using Events.TeamUpdated.Version1;
   using MongoDB.Driver;

   public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
   {
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public TeamUpdatedEventHandler(IMongoCollection<Invitation> invitationsCollection)
      {
         _invitationsCollection = invitationsCollection;
      }

      public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var filter = Builders<Invitation>.Filter.Where(m => m.Team.Id == @event.TeamId);
         var update = Builders<Invitation>.Update.Set(m => m.Team.Name, @event.Name);

         await _invitationsCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
      }
   }
}
