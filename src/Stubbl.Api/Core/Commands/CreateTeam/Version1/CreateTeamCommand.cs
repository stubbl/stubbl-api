namespace Stubbl.Api.Core.Commands.CreateTeam.Version1
{
   using Gunnsoft.Cqs.Commands;
   using Events.TeamCreated.Version1;

   public class CreateTeamCommand : ICommand<TeamCreatedEvent>
   {
      public CreateTeamCommand(string name)
      {
         Name = name;
      }

      public string Name { get; }
   }
}