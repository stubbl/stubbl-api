using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.MemberAlreadyInvitedToTeam.Version1
{
    public class MemberAlreadyInvitedToTeamException : Exception
    {
        public MemberAlreadyInvitedToTeamException(string emailAddress, ObjectId teamId)
            : base($"Member has already been invited to the team. EmailAddress='{emailAddress}' TeamID='{teamId}'")
        {
            EmailAddress = emailAddress;
            TeamId = teamId;
        }

        public string EmailAddress { get; }
        public ObjectId TeamId { get; }
    }
}