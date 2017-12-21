namespace Stubbl.Api.Core.CommandHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.CreateTeam.Version1;
   using Common.CommandHandlers;
   using Data;
   using Data.Collections.DefaultRoles;
   using Events.Shared.Version1;
   using Events.TeamCreated.Version1;
   using Exceptions.AdministratorRoleNotFound.Version1;
   using Exceptions.UserRoleNotFound.Version1;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Member = Data.Collections.Teams.Member;
   using Team = Data.Collections.Teams.Team;

   public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, TeamCreatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<DefaultRole> _defaultRolesCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CreateTeamCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<DefaultRole> defaultRolesCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _defaultRolesCollection = defaultRolesCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamCreatedEvent> HandleAsync(CreateTeamCommand command, CancellationToken cancellationToken)
      {
         var defaultRoles = await _defaultRolesCollection.Find(Builders<DefaultRole>.Filter.Empty)
            .ToListAsync(cancellationToken);

         var administratorRole = defaultRoles.SingleOrDefault(r => string.Equals(r.Name, DefaultRoleNames.Administrator, StringComparison.OrdinalIgnoreCase));

         if (administratorRole == null)
         {
            throw new AdministratorRoleNotFoundException();
         }

         var userRole = defaultRoles.SingleOrDefault(r => string.Equals(r.Name, DefaultRoleNames.User, StringComparison.OrdinalIgnoreCase));

         if (userRole == null)
         {
            throw new UserRoleNotFoundException();
         }

         var administratorRoleId = ObjectId.GenerateNewId();

         var team = new Team
         {
            Name = command.Name,
            Members = new[]
            {
               new Member
               {
                  Id = _authenticatedMemberAccessor.AuthenticatedMember.Id,
                  Name = _authenticatedMemberAccessor.AuthenticatedMember.Name,
                  EmailAddress = _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress,
                  Role = new Data.Collections.Teams.Role
                  {
                     Id = administratorRoleId,
                     Name = administratorRole.Name,
                     Permissions = administratorRole.Permissions
                  }
               }
            },
            Roles = new[]
            {
               new Data.Collections.Teams.Role
               {
                  Id = administratorRoleId,
                  IsDefault = true,
                  Name = administratorRole.Name,
                  Permissions = administratorRole.Permissions
               },
               new Data.Collections.Teams.Role
               {
                  Id = ObjectId.GenerateNewId(),
                  IsDefault = true,
                  Name = userRole.Name,
                  Permissions = userRole.Permissions
               }
            }
         };

         await _teamsCollection.InsertOneAsync(team, cancellationToken: cancellationToken);

         return new TeamCreatedEvent
         (
            team.Id,
            team.Name, 
            team.Members.Select(m => new Events.TeamCreated.Version1.Member
               (
                  m.Id, 
                  m.Name, 
                  m.EmailAddress, 
                  new Role
                  (
                     m.Role.Id,
                     m.Role.Name,
                     m.Role.Permissions.ToEventPermissions()
                  )
               ))
               .ToList(),
            team.Roles.Select(r => new Role
               (
                  r.Id,
                  r.Name,
                  r.Permissions.ToEventPermissions()
               ))
               .ToList()
         );
      }
   }
}