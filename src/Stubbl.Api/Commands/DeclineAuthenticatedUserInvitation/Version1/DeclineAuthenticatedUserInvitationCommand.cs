using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.AuthenticatedUserInvitationDeclined.Version1;

namespace Stubbl.Api.Commands.DeclineAuthenticatedUserInvitation.Version1
{
    public class DeclineAuthenticatedUserInvitationCommand : ICommand<AuthenticatedUserInvitationDeclinedEvent>
    {
        public DeclineAuthenticatedUserInvitationCommand(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}