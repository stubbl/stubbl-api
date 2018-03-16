namespace Stubbl.Api.Core.Commands.UpdateTeam.Version1
{
   using CodeContrib.Commands;
   using Events.TeamUpdated.Version1;
   using MongoDB.Bson;

   public class UpdateTeamCommand : ICommand<TeamUpdatedEvent>
   {
      public UpdateTeamCommand(ObjectId teamId, string name)
      {
         TeamId = teamId;
         Name = name;
      }

      public string Name { get; }
      public ObjectId TeamId { get; }
   }
}