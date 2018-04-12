using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.Commands;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CreateAuthenticatedUser.Version1;
using Stubbl.Api.Data.Collections.Users;
using Stubbl.Api.Events.AuthenticatedUserCreated.Version1;
using Stubbl.Api.Exceptions.EmailAdressAlreadyTaken.Version1;
using Stubbl.Api.Exceptions.UserAlreadyExists.Version1;

namespace Stubbl.Api.CommandHandlers
{
    public class
        CreateAuthenticatedUserCommandHandler : ICommandHandler<CreateAuthenticatedUserCommand,
            AuthenticatedUserCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IReadOnlyCollection<ISubAccessor> _subAccessors;
        private readonly IMongoCollection<User> _usersCollection;

        public CreateAuthenticatedUserCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IReadOnlyCollection<ISubAccessor> subAccessors, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _subAccessors = subAccessors;
            _usersCollection = usersCollection;
        }

        public async Task<AuthenticatedUserCreatedEvent> HandleAsync(CreateAuthenticatedUserCommand command,
            CancellationToken cancellationToken)
        {
            var sub = _subAccessors.First(sa => sa.Sub != null).Sub;

            if (await _usersCollection.CountAsync(u => u.Sub == sub,
                    cancellationToken: cancellationToken) > 0)
            {
                throw new UserAlreadyExistsException
                (
                    sub
                );
            }

            if (await _usersCollection.CountAsync(u => u.EmailAddress == command.EmailAddress,
                    cancellationToken: cancellationToken) > 0)
            {
                throw new EmailAddressAlreadyTakenException
                (
                    command.EmailAddress
                );
            }

            var user = new User
            {
                Sub = sub,
                Name = command.Name,
                EmailAddress = command.EmailAddress
            };

            await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

            _authenticatedUserAccessor.Reload();

            return new AuthenticatedUserCreatedEvent
            (
                command.Name,
                command.EmailAddress
            );
        }
    }
}