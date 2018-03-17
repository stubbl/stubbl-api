using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamDeleted.Version1
{
    public class TeamDeletedEvent : IEvent
    {
        public TeamDeletedEvent(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}