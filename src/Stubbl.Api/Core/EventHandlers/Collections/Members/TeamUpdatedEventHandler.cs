namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Gunnsoft.Cqs.EventHandlers;
   using Data.Collections.Members;
   using Events.TeamUpdated.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class TeamUpdatedEventHandler : IEventHandler<TeamUpdatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Member> membersCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _membersCollection = membersCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(TeamUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var memberIds = await _teamsCollection.Find(t => t.Id == @event.TeamId)
           .Project(t => t.Members.Select(m => m.Id).ToList())
           .SingleOrDefaultAsync(cancellationToken);

         var team = _authenticatedUserAccessor.AuthenticatedUser.Teams
            .Single(t => t.Id == @event.TeamId);

         team.Name = @event.Name;

         var filter = Builders<Member>.Filter.Where(m => memberIds.Contains(m.Id));
         var pullUpdate = Builders<Member>.Update.PullFilter(m => m.Teams, t => t.Id == team.Id);
         var pushUpdate = Builders<Member>.Update.Push(m => m.Teams, team);
         var requests = new[]
         {
            new UpdateOneModel<Member>(filter, pullUpdate),
            new UpdateOneModel<Member>(filter, pushUpdate),
         };

         await _membersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
      }
   }
}
