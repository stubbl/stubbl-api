using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberNotInvitedToTeam.Version1
{
    public class MemberNotInvitedToTeamException : Exception
    {
        public MemberNotInvitedToTeamException(ObjectId memberId, ObjectId invitationId)
            : base($"Member not invited to the team. MemberID='{memberId}' InvitationID='{invitationId}'")
        {
            MemberId = memberId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId MemberId { get; }
    }
}