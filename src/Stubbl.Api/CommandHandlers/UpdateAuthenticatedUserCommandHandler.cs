using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.UpdateAuthenticatedUser.Version1;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Events.AuthenticatedUserUpdated.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class
        UpdateAuthenticatedUserCommandHandler : ICommandHandler<UpdateAuthenticatedUserCommand,
            AuthenticatedUserUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Member> _membersCollection;

        public UpdateAuthenticatedUserCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Member> membersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _membersCollection = membersCollection;
        }

        public async Task<AuthenticatedUserUpdatedEvent> HandleAsync(UpdateAuthenticatedUserCommand command,
            CancellationToken cancellationToken)
        {
            var filter = Builders<Member>.Filter.Where(t => t.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
            var update = Builders<Member>.Update.Set(t => t.Name, command.Name)
                .Set(t => t.EmailAddress, command.EmailAddress);

            await _membersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new AuthenticatedUserUpdatedEvent
            (
                command.Name,
                command.EmailAddress
            );
        }
    }
}