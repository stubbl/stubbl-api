using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberAlreadyInvitedToTeam.Version1
{
    public class MemberAlreadyInvitedToTeamException : Exception
    {
        public MemberAlreadyInvitedToTeamException(ObjectId memberId, ObjectId teamId)
            : base($"Member has already been invited to the team. MemberID='{memberId}' TeamID='{teamId}'")
        {
            MemberId = memberId;
            TeamId = teamId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}