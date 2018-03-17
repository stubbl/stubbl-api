using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotManageStubs.Version1
{
    public class MemberCannotManageStubsException : Exception
    {
        public MemberCannotManageStubsException(ObjectId memberId, ObjectId teamId)
            : base($"Member cannot manage stubs. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}