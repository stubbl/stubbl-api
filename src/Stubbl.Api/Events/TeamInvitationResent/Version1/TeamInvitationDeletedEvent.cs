using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamInvitationResent.Version1
{
    public class TeamInvitationResentEvent : IEvent
    {
        public TeamInvitationResentEvent(ObjectId teamId, ObjectId invitationId)
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}