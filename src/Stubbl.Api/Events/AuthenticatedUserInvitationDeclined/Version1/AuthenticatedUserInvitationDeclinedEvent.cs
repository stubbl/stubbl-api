using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.AuthenticatedUserInvitationDeclined.Version1
{
    public class AuthenticatedUserInvitationDeclinedEvent : IEvent
    {
        public AuthenticatedUserInvitationDeclinedEvent(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}