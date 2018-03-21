using Stubbl.Api.Data.Collections.Users;

namespace Stubbl.Api.Authentication
{
    public interface IAuthenticatedUserAccessor
    {
        User AuthenticatedUser { get; }
    }
}