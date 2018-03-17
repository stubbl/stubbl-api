using Stubbl.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.TeamNotFound.Version1
{
    public class TeamNotFoundResponse : ErrorResponse
    {
        public TeamNotFoundResponse()
            : base("TeamNotFound", "The team cannot be found.")
        {
        }
    }
}