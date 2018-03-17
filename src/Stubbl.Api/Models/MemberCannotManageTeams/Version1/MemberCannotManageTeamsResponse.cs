using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberCannotManageTeams.Version1
{
    public class MemberCannotManageTeamsResponse : ErrorResponse
    {
        public MemberCannotManageTeamsResponse()
            : base("MemberCannotManageTeams", "The member cannot manage teams.")
        {
        }
    }
}