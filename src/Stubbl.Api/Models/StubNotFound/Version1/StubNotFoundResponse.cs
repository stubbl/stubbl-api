using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.StubNotFound.Version1
{
    public class StubNotFoundResponse : ErrorResponse
    {
        public StubNotFoundResponse()
            : base("StubNotFound", "The stub cannot be found.")
        {
        }
    }
}