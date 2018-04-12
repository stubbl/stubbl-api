using Gunnsoft.Cqs.Commands;
using Stubbl.Api.Events.AuthenticatedUserCreated.Version1;

namespace Stubbl.Api.Commands.CreateAuthenticatedUser.Version1
{
    public class CreateAuthenticatedUserCommand : ICommand<AuthenticatedUserCreatedEvent>
    {
        public CreateAuthenticatedUserCommand(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
        public string Name { get; }
    }
}