using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamCreated.Version1;
using User = Stubbl.Api.Data.Collections.Users.User;
using Role = Stubbl.Api.Data.Collections.Users.Role;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
    public class TeamCreatedEventHandler : IEventHandler<TeamCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<User> _usersCollection;

        public TeamCreatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _usersCollection = usersCollection;
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

            var authenticatedUserId = _authenticatedUserAccessor.AuthenticatedUser.Id;
            var filter = Builders<User>.Filter.Where(m => m.Id == authenticatedUserId);
            var update = Builders<User>.Update.Push(m => m.Teams, team);

            await _usersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}