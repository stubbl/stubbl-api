namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.Shared.Version1;
   using Commands.UpdateTeamRole.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Teams;
   using Events.TeamRoleUpdated.Version1;
   using Exceptions.MemberCannotManageTeams.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.RoleCannotBeUpdated.Version1;
   using Exceptions.RoleNotFound.Version1;
   using MongoDB.Driver;
   using Permission = Data.Collections.Shared.Permission;

   public class UpdateTeamRoleCommandHandler : ICommandHandler<UpdateTeamRoleCommand, TeamRoleUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public UpdateTeamRoleCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamRoleUpdatedEvent> HandleAsync(UpdateTeamRoleCommand command, CancellationToken cancellationToken)
      {
         var team = _authenticatedMemberAccessor.AuthenticatedMember.Teams.SingleOrDefault(t => t.Id == command.TeamId);

         if (team == null)
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               command.TeamId
            );
         }

         if (!team.Role.Permissions.Contains(Permission.ManageRoles))
         {
            throw new MemberCannotManageTeamsException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               team.Id
            );
         }

         var role = await _teamsCollection.Find(t => t.Id == command.TeamId)
            .Project(t => t.Roles.SingleOrDefault(r => r.Id == command.RoleId))
            .SingleOrDefaultAsync(cancellationToken);

         if (role == null)
         {
            throw new RoleNotFoundException
            (
               role.Id,
               team.Id
            );
         }

         if (role.IsDefault)
         {
            throw new RoleCannotBeUpdatedException
            (
               role.Id,
               team.Id
            );
         }

         role.Name = command.Name;
         role.Permissions = command.Permissions.ToDataPermissions();

         var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
         var pullUpdate = Builders<Team>.Update.PullFilter(t => t.Roles, r => r.Id == role.Id);
         var pushUpdate = Builders<Team>.Update.Push(m => m.Roles, role);
         var requests = new[]
         {
            new UpdateOneModel<Team>(filter, pullUpdate),
            new UpdateOneModel<Team>(filter, pushUpdate)
         };

         await _teamsCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);

         return new TeamRoleUpdatedEvent
         (
            team.Id,
            role.Id,
            command.Name,
            command.Permissions.ToEventPermissions()
         );
      }
   }
}