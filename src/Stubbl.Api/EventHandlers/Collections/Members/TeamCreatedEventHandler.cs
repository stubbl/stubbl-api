using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.EventHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamCreated.Version1;
using Member = Stubbl.Api.Data.Collections.Members.Member;
using Role = Stubbl.Api.Data.Collections.Members.Role;

namespace Stubbl.Api.EventHandlers.Collections.Members
{
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