using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberNotInvitedToTeam.Version1
{
    public class MemberNotInvitedToTeamResponse : ErrorResponse
    {
        public MemberNotInvitedToTeamResponse()
            : base("MemberNotInvitedToTeam", "The member hasn't been invited to the team.")
        {
        }
    }
}