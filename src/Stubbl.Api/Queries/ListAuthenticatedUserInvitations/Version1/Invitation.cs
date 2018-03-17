using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListAuthenticatedUserInvitations.Version1
{
    public class Invitation
    {
        public Invitation(string id, Team team, Role role, InvitationStatus status)
        {
            Id = id;
            Team = team;
            Role = role;
            Status = status;
        }

        public string Id { get; }
        public Role Role { get; }
        public InvitationStatus Status { get; }
        public Team Team { get; }
    }
}