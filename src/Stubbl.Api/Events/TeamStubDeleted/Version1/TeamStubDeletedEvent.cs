using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamStubDeleted.Version1
{
    public class TeamStubDeletedEvent : IEvent
    {
        public TeamStubDeletedEvent(ObjectId teamId, ObjectId stubId)
        {
            TeamId = teamId;
            StubId = stubId;
        }

        public ObjectId StubId { get; }
        public ObjectId TeamId { get; }
    }
}