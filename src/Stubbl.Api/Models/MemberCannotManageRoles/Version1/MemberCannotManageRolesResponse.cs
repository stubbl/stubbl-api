using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberCannotManageRoles.Version1
{
    public class MemberCannotManageRolesResponse : ErrorResponse
    {
        public MemberCannotManageRolesResponse()
            : base("MemberCannotManageRoles", "The member cannot manage roles.")
        {
        }
    }
}