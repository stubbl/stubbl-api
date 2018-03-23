using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotManageRoles.Version1
{
    public class MemberCannotManageRolesException : Exception
    {
        public MemberCannotManageRolesException(ObjectId memberId, ObjectId teamId)
            : base($"Member cannot manage roles. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}