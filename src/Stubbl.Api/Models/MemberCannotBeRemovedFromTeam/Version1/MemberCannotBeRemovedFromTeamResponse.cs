using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberCannotBeRemovedFromTeam.Version1
{
    public class MemberCannotBeRemovedFromTeamResponse : ErrorResponse
    {
        public MemberCannotBeRemovedFromTeamResponse()
            : base("MemberCannotBeRemovedFromTeam", "The member cannot be removed from the team.")
        {
        }
    }
}