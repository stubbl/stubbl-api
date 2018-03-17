using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamMemberRemoved.Version1
{
    public class TeamMemberRemovedEvent : IEvent
    {
        public TeamMemberRemovedEvent(ObjectId teamId, ObjectId memberId)
        {
            TeamId = teamId;
            MemberId = memberId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}