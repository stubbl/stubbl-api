using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.InvitationNotFound.Version1
{
    public class InvitationNotFoundResponse : ErrorResponse
    {
        public InvitationNotFoundResponse()
            : base("InvitationNotFound", "The invitation cannot be found.")
        {
        }
    }
}