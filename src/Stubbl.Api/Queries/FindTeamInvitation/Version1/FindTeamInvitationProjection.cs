using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.FindTeamInvitation.Version1
{
    public class FindTeamInvitationProjection : IProjection
    {
        public FindTeamInvitationProjection(string id, Role role, InvitationStatus status)
        {
            Id = id;
            Role = role;
            Status = status;
        }

        public string Id { get; }
        public Role Role { get; }
        public InvitationStatus Status { get; }
    }
}