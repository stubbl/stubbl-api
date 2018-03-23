using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.AdministratorRoleNotFound.Version1
{
    public class AdministratorRoleNotFoundResponse : ErrorResponse
    {
        public AdministratorRoleNotFoundResponse()
            : base("AdministratorRoleNotFound", "The administrator role cannot be found.")
        {
        }
    }
}