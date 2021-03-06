﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.CreateTeam.Version1;
using Stubbl.Api.Data;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.Shared.Version1;
using Stubbl.Api.Events.TeamCreated.Version1;
using Member = Stubbl.Api.Data.Collections.Teams.Member;
using Role = Stubbl.Api.Data.Collections.Teams.Role;

namespace Stubbl.Api.CommandHandlers
{
    public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, TeamCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Team> _teamsCollection;

        public CreateTeamCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamCreatedEvent> HandleAsync(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            var administratorRoleId = ObjectId.GenerateNewId();

            var team = new Team
            {
                Name = command.Name,
                Members = new[]
                {
                    new Member
                    {
                        Id = _authenticatedUserAccessor.AuthenticatedUser.Id,
                        Name = _authenticatedUserAccessor.AuthenticatedUser.Name,
                        EmailAddress = _authenticatedUserAccessor.AuthenticatedUser.EmailAddress,
                        Role = new Role
                        {
                            Id = administratorRoleId,
                            Name = DefaultRoles.Administrator.Name,
                            Permissions = DefaultRoles.Administrator.Permissions
                        }
                    }
                },
                Roles = new[]
                {
                    new Role
                    {
                        Id = administratorRoleId,
                        IsDefault = true,
                        Name = DefaultRoles.Administrator.Name,
                        Permissions = DefaultRoles.Administrator.Permissions
                    },
                    new Role
                    {
                        Id = ObjectId.GenerateNewId(),
                        IsDefault = true,
                        Name = DefaultRoles.User.Name,
                        Permissions = DefaultRoles.User.Permissions
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
                        new Events.TeamCreated.Version1.Role
                        (
                            m.Role.Id,
                            m.Role.Name,
                            m.Role.Permissions.ToEventPermissions()
                        )
                    ))
                    .ToList(),
                team.Roles.Select(r => new Events.TeamCreated.Version1.Role
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