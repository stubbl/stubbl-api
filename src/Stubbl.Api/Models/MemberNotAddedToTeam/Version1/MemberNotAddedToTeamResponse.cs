using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberNotAddedToTeam.Version1
{
    public class MemberNotAddedToTeamResponse : ErrorResponse
    {
        public MemberNotAddedToTeamResponse()
            : base("MemberNotAddedToTeam", "The member hasn't been added to the team.")
        {
        }
    }
}