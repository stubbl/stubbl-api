using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.InvitationNotFound.Version1
{
    public class InvitationNotFoundException : Exception
    {
        public InvitationNotFoundException(ObjectId invitationId, ObjectId teamId)
            : base($"Invitation not found. InvitationID='{invitationId}' TeamID='{teamId}'")
        {
            InvitationId = invitationId;
            TeamId = teamId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}