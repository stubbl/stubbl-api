using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamInvitationAccepted.Version1;

namespace Stubbl.Api.Commands.AcceptAuthenticatedUserTeamInvitation.Version1
{
    public class AcceptAuthenticatedUserTeamInvitationCommand : ICommand<TeamInvitationAcceptedEvent>
    {
        public AcceptAuthenticatedUserTeamInvitationCommand(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}