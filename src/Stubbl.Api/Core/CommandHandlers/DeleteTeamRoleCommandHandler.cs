namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.DeleteTeamRole.Version1;
   using Gunnsoft.Cqs.CommandHandlers;
   using Data.Collections.Teams;
   using Data.Collections.Shared;
   using Events.TeamRoleDeleted.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.MemberCannotManageMembers.Version1;
   using Exceptions.RoleCannotBeUpdated.Version1;
   using Exceptions.RoleNotFound.Version1;
   using MongoDB.Driver;

   public class DeleteTeamRoleCommandHandler : ICommandHandler<DeleteTeamRoleCommand, TeamRoleDeletedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public DeleteTeamRoleCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamRoleDeletedEvent> HandleAsync(DeleteTeamRoleCommand command, CancellationToken cancellationToken)
      {
         var team = _authenticatedUserAccessor.AuthenticatedUser.Teams.SingleOrDefault(t => t.Id == command.TeamId);

         if (team == null)
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               command.TeamId
            );
         }

         if (!team.Role.Permissions.Contains(Permission.ManageMembers))
         {
            throw new MemberCannotManageMembersException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
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
               command.RoleId,
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

         var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
         var update = Builders<Team>.Update.PullFilter(t => t.Roles, r => r.Id == role.Id);

         await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new TeamRoleDeletedEvent
         (
            role.Id,
            team.Id
         );
      }
   }
}