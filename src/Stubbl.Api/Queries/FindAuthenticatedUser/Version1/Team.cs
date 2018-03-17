namespace Stubbl.Api.Queries.FindAuthenticatedUser.Version1
{
    public class Team
    {
        public Team(string id, string name, Role role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public string Id { get; }
        public string Name { get; }
        public Role Role { get; }
    }
}