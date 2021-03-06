using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberAlreadyInvitedToTeam.Version1
{
    public class MemberAlreadyInvitedToTeamResponse : ErrorResponse
    {
        public MemberAlreadyInvitedToTeamResponse()
            : base("MemberAlreadyInvitedToTeam", "The member has already been invited to the team.")
        {
        }
    }
}