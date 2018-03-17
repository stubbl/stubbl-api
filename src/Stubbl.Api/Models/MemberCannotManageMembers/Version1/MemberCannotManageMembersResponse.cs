using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberCannotManageMembers.Version1
{
    public class MemberCannotManageMembersResponse : ErrorResponse
    {
        public MemberCannotManageMembersResponse()
            : base("MemberCannotManageMembers", "The member cannot manage members.")
        {
        }
    }
}