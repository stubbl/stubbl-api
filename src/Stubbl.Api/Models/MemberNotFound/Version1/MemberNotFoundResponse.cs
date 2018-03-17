using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.MemberNotFound.Version1
{
    public class MemberNotFoundResponse : ErrorResponse
    {
        public MemberNotFoundResponse()
            : base("MemberNotFound", "The member cannot be found.")
        {
        }
    }
}