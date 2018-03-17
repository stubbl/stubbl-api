using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.RoleNotFound.Version1
{
    public class RoleNotFoundResponse : ErrorResponse
    {
        public RoleNotFoundResponse()
            : base("RoleNotFound", "The role cannot be found.")
        {
        }
    }
}