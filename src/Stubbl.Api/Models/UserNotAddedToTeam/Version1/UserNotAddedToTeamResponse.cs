using Gunnsoft.Api.Models.Error.Version1;

namespace Stubbl.Api.Models.UserNotAddedToTeam.Version1
{
    public class UserNotAddedToTeamResponse : ErrorResponse
    {
        public UserNotAddedToTeamResponse()
            : base("UserNotAddedToTeam", "The user hasn't been added to the team.")
        {
        }
    }
}