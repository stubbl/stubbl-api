using MongoDB.Bson;

namespace Stubbl.Api.Events.TeamCreated.Version1
{
    public class Member
    {
        public Member(ObjectId id, string name, string emailAddress, Role role)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            Role = role;
        }

        public string EmailAddress { get; }
        public ObjectId Id { get; }
        public string Name { get; }
        public Role Role { get; }
    }
}