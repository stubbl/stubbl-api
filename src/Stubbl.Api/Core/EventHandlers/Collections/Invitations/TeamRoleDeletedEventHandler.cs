namespace Stubbl.Api.Core.EventHandlers.Collections.Invitations
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.EventHandlers;
   using Data;
   using Data.Collections.Invitations;
   using Events.TeamRoleDeleted.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class TeamRoleDeletedEventHandler : IEventHandler<TeamRoleDeletedEvent>
   {
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamRoleDeletedEventHandler(IMongoCollection<Invitation> invitationsCollection,
         IMongoCollection<Team> teamsCollection)
      {
         _invitationsCollection = invitationsCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(TeamRoleDeletedEvent @event, CancellationToken cancellationToken)
      {
         var userRole = await _teamsCollection.Find(t => t.Id == @event.TeamId)
            .Project(t => t.Roles.Single(r => r.Name.ToLower() == DefaultRoleNames.User.ToLower() && r.IsDefault))
            .SingleAsync(cancellationToken);

         var filter = Builders<Invitation>.Filter.Where(i => i.Team.Id == @event.TeamId && i.Role.Id == @event.RoleId);
         var update = Builders<Invitation>.Update.Set(i => i.Role.Id, userRole.Id)
            .Set(i => i.Role.Name, userRole.Name);

         await _invitationsCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
      }
   }
}
