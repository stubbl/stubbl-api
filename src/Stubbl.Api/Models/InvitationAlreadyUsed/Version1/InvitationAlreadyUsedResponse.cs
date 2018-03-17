using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.InvitationAlreadyUsed.Version1
{
    public class InvitationAlreadyUsedResponse : ErrorResponse
    {
        public InvitationAlreadyUsedResponse()
            : base("InvitationAlreadyUsed", "The invitation has already been used.")
        {
        }
    }
}