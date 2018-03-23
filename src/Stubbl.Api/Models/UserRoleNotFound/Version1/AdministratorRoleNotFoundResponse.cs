using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.UserRoleNotFound.Version1
{
    public class UserRoleNotFoundResponse : ErrorResponse
    {
        public UserRoleNotFoundResponse()
            : base("UserRoleNotFound", "The user role cannot be found.")
        {
        }
    }
}