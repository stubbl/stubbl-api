using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotManageMembers.Version1
{
    public class MemberCannotManageMembersException : Exception
    {
        public MemberCannotManageMembersException(ObjectId memberId, ObjectId teamId)
            : base($"AuthenticatedUser cannot manage team members. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}