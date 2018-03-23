using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.UserNotAddedToTeam.Version1
{
    public class UserNotAddedToTeamException : Exception
    {
        public UserNotAddedToTeamException(ObjectId memberId, ObjectId teamId)
            : base($"User has not been added to the team. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}