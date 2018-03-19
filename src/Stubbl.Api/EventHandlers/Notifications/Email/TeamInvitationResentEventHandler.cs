using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Commands.SendEmail.Version1;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Events.TeamInvitationResent.Version1;
using Stubbl.Api.Notifications.Email.TeamInvitation.Version1;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.EventHandlers.Notifications.Email
{
    public class TeamInvitationResentEventHandler : IEventHandler<TeamInvitationResentEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMongoCollection<Invitation> _invitationsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;

        public TeamInvitationResentEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            ICommandDispatcher commandDispatcher, IMongoCollection<Invitation> invitationsCollection,
            IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _commandDispatcher = commandDispatcher;
            _invitationsCollection = invitationsCollection;
            _teamsCollection = teamsCollection;
        }

        public async Task HandleAsync(TeamInvitationResentEvent @event, CancellationToken cancellationToken)
        {
            var teamName = await _teamsCollection.Find(t => t.Id == @event.TeamId)
                .Project(t => t.Name)
                .SingleAsync(cancellationToken);
            var emailAddress = await _invitationsCollection.Find(t => t.Id == @event.InvitationId)
                .Project(i => i.EmailAddress)
                .SingleAsync(cancellationToken);

            var email = new TeamInvitationEmail
            (
                emailAddress,
                teamName,
                _authenticatedUserAccessor.AuthenticatedUser.Name,
                @event.InvitationId
            );

            var command = new SendEmailCommand
            (
                email
            );

            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
    }
}