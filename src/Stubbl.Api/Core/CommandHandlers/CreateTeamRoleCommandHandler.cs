namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.CreateTeamRole.Version1;
   using Commands.Shared.Version1;
   using CodeContrib.CommandHandlers;
   using Data.Collections.Teams;
   using Events.Shared.Version1;
   using Events.TeamRoleCreated.Version1;
   using Exceptions.MemberCannotManageRoles.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.RoleAlreadyExists.Version1;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Permission = Data.Collections.Shared.Permission;

   public class CreateTeamRoleCommandHandler : ICommandHandler<CreateTeamRoleCommand, TeamRoleCreatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CreateTeamRoleCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamRoleCreatedEvent> HandleAsync(CreateTeamRoleCommand command, CancellationToken cancellationToken)
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

         if (!team.Role.Permissions.Contains(Permission.ManageRoles))
         {
            throw new MemberCannotManageRolesException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               team.Id
            );
         }

         if (await _teamsCollection.Find(t =>t.Id == team.Id && t.Roles.Any(r => r.Name.ToLower() == command.Name.ToLower())).AnyAsync(cancellationToken))
         {
            throw new RoleAlreadyExistsException
            (
               command.Name,
               team.Id
            );
         }

         var role = new Role
         {
            Id = ObjectId.GenerateNewId(),
            Name = command.Name,
            Permissions = command.Permissions.ToDataPermissions()
         };

         var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
         var update = Builders<Team>.Update.Push(t => t.Roles, role);

         await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new TeamRoleCreatedEvent
         (
            role.Id,
            team.Id,
            role.Name,
            role.Permissions.ToEventPermissions()
         );
      }
   }
}