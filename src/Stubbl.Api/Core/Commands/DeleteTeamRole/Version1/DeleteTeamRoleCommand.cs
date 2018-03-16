namespace Stubbl.Api.Core.Commands.DeleteTeamRole.Version1
{
   using CodeContrib.Commands;
   using Events.TeamRoleDeleted.Version1;
   using MongoDB.Bson;

   public class DeleteTeamRoleCommand : ICommand<TeamRoleDeletedEvent>
   {
      public DeleteTeamRoleCommand(ObjectId teamId, ObjectId roleId)
      {
         TeamId = teamId;
         RoleId = roleId;
      }

      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}