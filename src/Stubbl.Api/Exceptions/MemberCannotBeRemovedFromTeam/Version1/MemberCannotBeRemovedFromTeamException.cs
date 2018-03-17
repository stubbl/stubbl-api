using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotBeRemovedFromTeam.Version1
{
    public class MemberCannotBeRemovedFromTeamException : Exception
    {
        public MemberCannotBeRemovedFromTeamException(ObjectId memberId, ObjectId teamId)
            : base($"Member cannot be removed from the team. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
        public ObjectId MemberId { get; }
    }
}