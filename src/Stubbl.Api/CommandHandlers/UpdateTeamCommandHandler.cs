﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.UpdateTeam.Version1;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Events.TeamUpdated.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.CommandHandlers
{
    public class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, TeamUpdatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Member> _membersCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public UpdateTeamCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Member> membersCollection, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _membersCollection = membersCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamUpdatedEvent> HandleAsync(UpdateTeamCommand command, CancellationToken cancellationToken)
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

            if (!team.Role.Permissions.Contains(Permission.ManageTeams))
            {
                throw new MemberCannotManageTeamsException
                (
                    _authenticatedUserAccessor.AuthenticatedUser.Id,
                    team.Id
                );
            }

            var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
            var update = Builders<Team>.Update.Set(t => t.Name, command.Name);

            var result = await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return new TeamUpdatedEvent
            (
                team.Id,
                command.Name
            );
        }
    }
}