using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamInvitationCreated.Version1;

namespace Stubbl.Api.Commands.CreateTeamInvitation.Version1
{
    public class CreateTeamInvitationCommand : ICommand<TeamInvitationCreatedEvent>
    {
        public CreateTeamInvitationCommand(ObjectId teamId, ObjectId roleId, string emailAddress)
        {
            TeamId = teamId;
            RoleId = roleId;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}