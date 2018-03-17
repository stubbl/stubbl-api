using Stubbl.Api.Data.Collections.Members;

namespace Stubbl.Api.Authentication
{
    public interface IAuthenticatedUserAccessor
    {
        Member AuthenticatedUser { get; }
    }
}