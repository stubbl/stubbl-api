using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberCannotManageInvitations.Version1
{
    public class MemberCannotManageInvitationsException : Exception
    {
        public MemberCannotManageInvitationsException(ObjectId memberId, ObjectId teamId)
            : base($"Member cannot manage invitations. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}