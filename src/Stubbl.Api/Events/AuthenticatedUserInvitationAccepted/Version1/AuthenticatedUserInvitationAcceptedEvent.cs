using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1
{
    public class AuthenticatedUserInvitationAcceptedEvent : IEvent
    {
        public AuthenticatedUserInvitationAcceptedEvent(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}