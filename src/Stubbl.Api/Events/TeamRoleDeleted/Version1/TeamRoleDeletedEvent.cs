using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamRoleDeleted.Version1
{
    public class TeamRoleDeletedEvent : IEvent
    {
        public TeamRoleDeletedEvent(ObjectId roleId, ObjectId teamId)
        {
            RoleId = roleId;
            TeamId = teamId;
        }

        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}