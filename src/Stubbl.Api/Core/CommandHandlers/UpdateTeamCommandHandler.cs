namespace Stubbl.Api.Core.CommandHandlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Authentication;
    using Commands.UpdateTeam.Version1;
    using Gunnsoft.Cqs.CommandHandlers;
    using Data.Collections.Shared;
    using Data.Collections.Teams;
    using Events.TeamUpdated.Version1;
    using Exceptions.MemberCannotManageTeams.Version1;
    using Exceptions.MemberNotAddedToTeam.Version1;
    using MongoDB.Driver;
    using Member = Data.Collections.Members.Member;

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