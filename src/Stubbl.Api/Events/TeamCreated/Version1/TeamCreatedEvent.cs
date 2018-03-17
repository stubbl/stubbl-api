using System.Collections.Generic;
using Gunnsoft.Cqs.Events;
using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamCreated.Version1
{
    public class TeamCreatedEvent : IEvent
    {
        public TeamCreatedEvent(ObjectId teamId, string name, IReadOnlyCollection<Member> members,
            IReadOnlyCollection<Role> roles)
        {
            TeamId = teamId;
            Name = name;
            Members = members;
            Roles = roles;
        }

        public string Name { get; }
        public IReadOnlyCollection<Member> Members { get; }
        public IReadOnlyCollection<Role> Roles { get; }
        public ObjectId TeamId { get; }
    }
}