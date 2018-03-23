using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.UpdateAuthenticatedUser.Version1;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.AuthenticatedUserUpdated.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class
        UpdateAuthenticatedUserCommandHandler : ICommandHandler<UpdateAuthenticatedUserCommand,
            AuthenticatedUserUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<User> _usersCollection;

        public UpdateAuthenticatedUserCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _usersCollection = usersCollection;
        }

        public async Task<AuthenticatedUserUpdatedEvent> HandleAsync(UpdateAuthenticatedUserCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<User>.Filter.Where(t => t.Id == _authenticatedUserAccessor.AuthenticatedUser.Id);
                var update = Builders<User>.Update.Set(t => t.Name, command.Name)
                    .Set(t => t.EmailAddress, command.EmailAddress);

                await _usersCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            }
            catch (AuthenticatedUserNotFoundException exception)
            {
                var user = new User
                {
                    Sub = exception.Sub,
                    Name = command.Name,
                    EmailAddress = command.EmailAddress
                };

                await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
            }

            return new AuthenticatedUserUpdatedEvent
            (
                command.Name,
                command.EmailAddress
            );
        }
    }
}