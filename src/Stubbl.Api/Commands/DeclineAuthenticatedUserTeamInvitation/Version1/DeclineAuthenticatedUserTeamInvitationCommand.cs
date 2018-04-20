using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamInvitationDeclined.Version1;

namespace Stubbl.Api.Commands.DeclineAuthenticatedUserTeamInvitation.Version1
{
    public class DeclineAuthenticatedUserTeamInvitationCommand : ICommand<TeamInvitationDeclinedEvent>
    {
        public DeclineAuthenticatedUserTeamInvitationCommand(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}