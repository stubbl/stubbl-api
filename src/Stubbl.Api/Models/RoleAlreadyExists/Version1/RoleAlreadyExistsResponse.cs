using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.RoleAlreadyExists.Version1
{
    public class RoleAlreadyExistsResponse : ErrorResponse
    {
        public RoleAlreadyExistsResponse()
            : base("RoleAlreadyExists", "The role already exists.")
        {
        }
    }
}