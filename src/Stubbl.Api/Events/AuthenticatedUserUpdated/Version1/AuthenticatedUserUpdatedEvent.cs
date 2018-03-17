using Gunnsoft.Cqs.Events;

namespace Stubbl.Api.Events.AuthenticatedUserUpdated.Version1
{
    public class AuthenticatedUserUpdatedEvent : IEvent
    {
        public AuthenticatedUserUpdatedEvent(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
        public string Name { get; }
    }
}