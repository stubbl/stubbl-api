namespace Stubbl.Api.Core.Commands.UpdateTeamRole.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Commands;
   using Events.TeamRoleUpdated.Version1;
   using MongoDB.Bson;
   using Shared.Version1;

   public class UpdateTeamRoleCommand : ICommand<TeamRoleUpdatedEvent>
   {
      public UpdateTeamRoleCommand(ObjectId teamId, ObjectId roleId, string name,IReadOnlyCollection<Permission> permissions)
      {
         TeamId = teamId;
         RoleId = roleId;
         Name = name;
         Permissions = permissions;
      }

      public string Name { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}