using Gunnsoft.Cqs.Commands;
using Stubbl.Api.Events.AuthenticatedUserUpdated.Version1;

namespace Stubbl.Api.Commands.UpdateAuthenticatedUser.Version1
{
    public class UpdateAuthenticatedUserCommand : ICommand<AuthenticatedUserUpdatedEvent>
    {
        public UpdateAuthenticatedUserCommand(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
        public string Name { get; }
    }
}