using Gunnsoft.Cqs.Commands;
using Stubbl.Api.Events.TeamCreated.Version1;

namespace Stubbl.Api.Commands.CreateTeam.Version1
{
    public class CreateTeamCommand : ICommand<TeamCreatedEvent>
    {
        public CreateTeamCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}