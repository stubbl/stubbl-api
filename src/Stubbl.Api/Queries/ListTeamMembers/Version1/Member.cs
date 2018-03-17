namespace Stubbl.Api.Queries.ListTeamMembers.Version1
{
    public class Member
    {
        public Member(string id, string name, string emailAddress, Role role)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            Role = role;
        }

        public string EmailAddress { get; }
        public string Id { get; }
        public string Name { get; }
        public Role Role { get; }
    }
}