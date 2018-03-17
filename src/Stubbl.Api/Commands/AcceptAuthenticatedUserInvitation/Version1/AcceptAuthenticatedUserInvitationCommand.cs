using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.AuthenticatedUserInvitationAccepted.Version1;

namespace Stubbl.Api.Commands.AcceptAuthenticatedUserInvitation.Version1
{
    public class AcceptAuthenticatedUserInvitationCommand : ICommand<AuthenticatedUserInvitationAcceptedEvent>
    {
        public AcceptAuthenticatedUserInvitationCommand(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}