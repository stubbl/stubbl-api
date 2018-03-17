using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.InvitationAlreadyUsed.Version1
{
    public class InvitationAlreadyUsedException : Exception
    {
        public InvitationAlreadyUsedException(ObjectId invitationId, ObjectId teamId)
            : base($"Invitation already used. InvitationID='{invitationId}' TeamID='{teamId}'")
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}