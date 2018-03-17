using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using Stubbl.Api.Events.TeamMemberRemoved.Version1;

namespace Stubbl.Api.Commands.RemoveTeamMember.Version1
{
    public class RemoveTeamMemberCommand : ICommand<TeamMemberRemovedEvent>
    {
        public RemoveTeamMemberCommand(ObjectId teamId, ObjectId memberId)
        {
            TeamId = teamId;
            MemberId = memberId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}