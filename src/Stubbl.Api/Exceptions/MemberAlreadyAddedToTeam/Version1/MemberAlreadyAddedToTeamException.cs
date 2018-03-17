using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberAlreadyAddedToTeam.Version1
{
    public class MemberAlreadyAddedToTeamException : Exception
    {
        public MemberAlreadyAddedToTeamException(ObjectId memberId, ObjectId teamId)
            : base($"Member has already been added to the team. MemberID='{memberId}' TeamID='{teamId}'")
        {
            TeamId = teamId;
            MemberId = memberId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}