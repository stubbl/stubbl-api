using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberCannotManageInvitations.Version1
{
    public class MemberCannotManageInvitationsResponse : ErrorResponse
    {
        public MemberCannotManageInvitationsResponse()
            : base("MemberCannotInvitationsMembers", "The member cannot manage invitations.")
        {
        }
    }
}