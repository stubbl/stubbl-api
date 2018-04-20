using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamInvitationAccepted.Version1
{
    public class TeamInvitationAcceptedEvent : IEvent
    {
        public TeamInvitationAcceptedEvent(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}