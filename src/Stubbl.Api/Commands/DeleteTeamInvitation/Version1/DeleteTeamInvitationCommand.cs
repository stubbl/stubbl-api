using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamInvitationDeleted.Version1;

namespace Stubbl.Api.Commands.DeleteTeamInvitation.Version1
{
    public class DeleteTeamInvitationCommand : ICommand<TeamInvitationDeletedEvent>
    {
        public DeleteTeamInvitationCommand(ObjectId teamId, ObjectId invitationId)
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}