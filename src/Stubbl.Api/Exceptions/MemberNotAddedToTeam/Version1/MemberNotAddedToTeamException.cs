using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1
{
    public class MemberNotAddedToTeamException : Exception
    {
        public MemberNotAddedToTeamException(ObjectId memberId, ObjectId teamId)
            : base($"AuthenticatedUser has not been added to the team. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}