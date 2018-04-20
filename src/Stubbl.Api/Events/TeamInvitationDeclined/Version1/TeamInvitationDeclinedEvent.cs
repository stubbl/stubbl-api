using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamInvitationDeclined.Version1
{
    public class TeamInvitationDeclinedEvent : IEvent
    {
        public TeamInvitationDeclinedEvent(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}