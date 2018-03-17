using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.TeamUpdated.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
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
                new UpdateOneModel<Member>(filter, pushUpdate)
            };

            await _membersCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);
        }
    }
}