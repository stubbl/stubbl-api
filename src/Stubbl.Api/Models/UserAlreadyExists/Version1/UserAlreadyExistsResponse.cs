using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.UserAlreadyExists.Version1
{
    public class UserAlreadyExistsResponse : ErrorResponse
    {
        public UserAlreadyExistsResponse()
            : base("UserAlreadyExists", "The user already exists.")
        {
        }
    }
}