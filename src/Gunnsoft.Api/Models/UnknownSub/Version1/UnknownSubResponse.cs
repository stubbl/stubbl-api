using Gunnsoft.Api.Models.Error.Version1;

namespace Gunnsoft.Api.Models.UnknownSub.Version1
{
    public class UnknownSubResponse : ErrorResponse
    {
        public UnknownSubResponse()
            : base("UnknownSub", "The sub must be specified using the 'sub' claim.")
        {
        }
    }
}