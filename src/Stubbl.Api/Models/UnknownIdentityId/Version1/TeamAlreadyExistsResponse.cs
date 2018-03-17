using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.UnknownIdentityId.Version1
{
    public class UnknownIdentityIdResponse : ErrorResponse
    {
        public UnknownIdentityIdResponse()
            : base("UnknownIdentityId", "The Identity ID must be specified using the 'sub' claim.")
        {
        }
    }
}