using Gunnsoft.Cqs.Events;

namespace Stubbl.Api.Events.AuthenticatedUserCreated.Version1
{
    public class AuthenticatedUserCreatedEvent : IEvent
    {
        public AuthenticatedUserCreatedEvent(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
        public string Name { get; }
    }
}