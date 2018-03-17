﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Cqs.CommandHandlers;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.DeleteTeam.Version1;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Data.Collections.Shared;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Events.TeamDeleted.Version1;
using Stubbl.Api.Exceptions.MemberCannotManageTeams.Version1;
using Stubbl.Api.Exceptions.MemberNotAddedToTeam.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.CommandHandlers
{
    public class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, TeamDeletedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IMongoCollection<Log> _logsCollection;
        private readonly IMongoCollection<Member> _membersCollection;
        private readonly IMongoCollection<Stub> _stubsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public DeleteTeamCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IMongoCollection<Log> logsCollection, IMongoCollection<Member> membersCollection,
            IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _logsCollection = logsCollection;
            _membersCollection = membersCollection;
            _stubsCollection = stubsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task<TeamDeletedEvent> HandleAsync(DeleteTeamCommand command, CancellationToken cancellationToken)
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

            await _teamsCollection.DeleteOneAsync(filter, cancellationToken);

            return new TeamDeletedEvent
            (
                team.Id
            );
        }
    }
}