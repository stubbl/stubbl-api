using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.UserNotInvitedToTeam.Version1
{
    public class UserNotInvitedToTeamException : Exception
    {
        public UserNotInvitedToTeamException(ObjectId memberId, ObjectId invitationId)
            : base($"User not invited to the team. MemberID='{memberId}' InvitationID='{invitationId}'")
        {
            MemberId = memberId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId MemberId { get; }
    }
}