using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamInvitationResent.Version1;

namespace Stubbl.Api.Commands.ResendTeamInvitation.Version1
{
    public class ResendTeamInvitationCommand : ICommand<TeamInvitationResentEvent>
    {
        public ResendTeamInvitationCommand(ObjectId teamId, ObjectId invitationId)
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}