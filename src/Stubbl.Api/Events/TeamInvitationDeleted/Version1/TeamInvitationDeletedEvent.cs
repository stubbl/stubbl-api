using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamInvitationDeleted.Version1
{
    public class TeamInvitationDeletedEvent : IEvent
    {
        public TeamInvitationDeletedEvent(ObjectId teamId, ObjectId invitationId)
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}