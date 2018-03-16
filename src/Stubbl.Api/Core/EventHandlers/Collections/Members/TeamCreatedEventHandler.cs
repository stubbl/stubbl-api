namespace Stubbl.Api.Core.EventHandlers.Collections.Members
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.EventHandlers;
   using Events.Shared.Version1;
   using Events.TeamCreated.Version1;
   using MongoDB.Driver;
   using Member = Data.Collections.Members.Member;
   using Role = Data.Collections.Members.Role;
   using Team = Data.Collections.Members.Team;

   public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Member> _membersCollection;

      public TeamCreatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Member> membersCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _membersCollection = membersCollection;
      }

      public async Task HandleAsync(TeamCreatedEvent @event, CancellationToken cancellationToken)
      {
         var member = @event.Members.Single(m => m.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);

         var team = new Team
         {
            Id = @event.TeamId,
            Name = @event.Name,
            Role = new Role
            {
               Id = member.Role.Id,
               Name = member.Role.Name,
               Permissions = member.Role.Permissions.ToDataPermissions()
            }
         };

         var filter = Builders<Member>.Filter.Where(m => m.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
         var update = Builders<Member>.Update.Push(m => m.Teams, team);

         await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
      }
   }
}
