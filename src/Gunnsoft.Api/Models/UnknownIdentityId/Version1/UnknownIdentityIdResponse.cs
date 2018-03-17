using Gunnsoft.Api.Models.Error.Version1;

namespace Gunnsoft.Api.Models.UnknownIdentityId.Version1
{
    public class UnknownIdentityIdResponse : ErrorResponse
    {
        public UnknownIdentityIdResponse()
            : base("UnknownIdentityId", "The Identity ID must be specified using the 'sub' claim.")
        {
        }
    }
}