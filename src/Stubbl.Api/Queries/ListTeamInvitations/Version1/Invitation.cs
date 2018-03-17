using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListTeamInvitations.Version1
{
    public class Invitation
    {
        public Invitation(string id, string teamId, Role role, InvitationStatus status, string emailAddress)
        {
            Id = id;
            TeamId = teamId;
            Role = role;
            Status = status;
            EmailAddress = emailAddress;
        }

        public string Id { get; }
        public Role Role { get; }
        public string EmailAddress { get; }
        public InvitationStatus Status { get; }
        public string TeamId { get; }
    }
}