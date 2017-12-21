namespace Stubbl.Api.Core.EventHandlers.Collections.Teams
{
   using System.Linq;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.EventHandlers;
   using Data.Collections.Teams;
   using Events.AuthenticatedMemberUpdated.Version1;
   using MongoDB.Driver;

   public class AuthenticatedMemberUpdatedEventHandler : IEventHandler<AuthenticatedMemberUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public AuthenticatedMemberUpdatedEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(AuthenticatedMemberUpdatedEvent @event, CancellationToken cancellationToken)
      {
         var requests = new List<WriteModel<Team>>();

         foreach (var teamId in _authenticatedMemberAccessor.AuthenticatedMember.Teams.Select(t => t.Id))
         {
            var member = new Member
            {
               Name = @event.Name,
               EmailAddress = @event.EmailAddress
            };

            var filter = Builders<Team>.Filter.Where(t => t.Id == teamId);
            var pullUpdate = Builders<Team>.Update.PullFilter(t => t.Members, t => t.Id == _authenticatedMemberAccessor.AuthenticatedMember.Id);
            var pushUpdate = Builders<Team>.Update.Push(t => t.Members, member);

            requests.Add(new UpdateOneModel<Team>(filter, pullUpdate));
            requests.Add(new UpdateOneModel<Team>(filter, pushUpdate));
         }

         await _teamsCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
      }
   }
}
