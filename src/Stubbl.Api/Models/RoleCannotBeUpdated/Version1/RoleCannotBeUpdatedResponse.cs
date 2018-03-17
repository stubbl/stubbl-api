using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.RoleCannotBeUpdated.Version1
{
    public class RoleCannotBeUpdatedResponse : ErrorResponse
    {
        public RoleCannotBeUpdatedResponse()
            : base("RoleNotFound", "The role cannot be updated.")
        {
        }
    }
}