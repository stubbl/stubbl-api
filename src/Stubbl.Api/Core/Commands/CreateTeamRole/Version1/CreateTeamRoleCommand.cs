namespace Stubbl.Api.Core.Commands.CreateTeamRole.Version1
{
   using System.Collections.Generic;
   using Common.Commands;
   using Events.TeamRoleCreated.Version1;
   using MongoDB.Bson;
   using Shared.Version1;

   public class CreateTeamRoleCommand : ICommand<TeamRoleCreatedEvent>
   {
      public CreateTeamRoleCommand(ObjectId teamId, string name,
         IReadOnlyCollection<Permission> permissions)
      {
         TeamId = teamId;
         Name = name;
         Permissions = permissions;
      }

      public string Name { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
      public ObjectId TeamId { get; }
   }
}