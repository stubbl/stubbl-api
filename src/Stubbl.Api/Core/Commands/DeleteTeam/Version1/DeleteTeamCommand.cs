namespace Stubbl.Api.Core.Commands.DeleteTeam.Version1
{
   using CodeContrib.Commands;
   using Events.TeamDeleted.Version1;
   using MongoDB.Bson;

   public class DeleteTeamCommand : ICommand<TeamDeletedEvent>
   {
      public DeleteTeamCommand(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}