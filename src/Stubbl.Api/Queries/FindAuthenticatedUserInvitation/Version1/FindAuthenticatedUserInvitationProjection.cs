using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1
{
    public class FindAuthenticatedUserInvitationProjection : IProjection
    {
        public FindAuthenticatedUserInvitationProjection(string id, Team team, Role role)
        {
            Id = id;
            Team = team;
            Role = role;
        }

        public string Id { get; }
        public Role Role { get; }
        public Team Team { get; }
    }
}