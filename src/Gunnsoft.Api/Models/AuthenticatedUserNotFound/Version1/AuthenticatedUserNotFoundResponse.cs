using Gunnsoft.Api.Models.Error.Version1;

namespace Gunnsoft.Api.Models.AuthenticatedUserNotFound.Version1
{
    public class AuthenticatedUserNotFoundResponse : ErrorResponse
    {
        public AuthenticatedUserNotFoundResponse()
            : base("AuthenticatedUserNotFound", "The authenticted member cannot be found.")
        {
        }
    }
}