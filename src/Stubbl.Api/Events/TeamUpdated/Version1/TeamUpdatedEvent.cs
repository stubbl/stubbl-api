using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamUpdated.Version1
{
    public class TeamUpdatedEvent : IEvent
    {
        public TeamUpdatedEvent(ObjectId teamId, string name)
        {
            TeamId = teamId;
            Name = name;
        }

        public string Name { get; }
        public ObjectId TeamId { get; }
    }
}