using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1
{
    public class MemberCannotManageTeamsException : Exception
    {
        public MemberCannotManageTeamsException(ObjectId memberId, ObjectId teamId)
            : base($"Member cannot manage teams. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}