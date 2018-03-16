namespace Stubbl.Api.Core.EventHandlers.Collections.Teams
{
   using System.Linq;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using CodeContrib.EventHandlers;
   using Data.Collections.Teams;
   using Events.AuthenticatedUserUpdated.Version1;
   using MongoDB.Driver;

   public class AuthenticatedUserUpdatedEventHandler : IEventHandler<AuthenticatedUserUpdatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public AuthenticatedUserUpdatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(AuthenticatedUserUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var requests = new List<WriteModel<Team>>();

         foreach (var teamId in _authenticatedUserAccessor.AuthenticatedUser.Teams.Select(t => t.Id))
         {
            var member = new Member
            {
               Name = @event.Name,
               EmailAddress = @event.EmailAddress
            };

            var filter = Builders<Team>.Filter.Where(t => t.Id == teamId);
            var pullUpdate = Builders<Team>.Update.PullFilter(t => t.Members, t => t.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
            var pushUpdate = Builders<Team>.Update.Push(t => t.Members, member);

            requests.Add(new UpdateOneModel<Team>(filter, pullUpdate));
            requests.Add(new UpdateOneModel<Team>(filter, pushUpdate));
         }

         await _teamsCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
      }
   }
}
