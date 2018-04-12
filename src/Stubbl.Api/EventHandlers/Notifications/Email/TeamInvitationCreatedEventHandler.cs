using System.Threading;
using System.Threading.Tasks;
using Gunnsoft.Cqs.Commands;
using Gunnsoft.Cqs.Events;
using MongoDB.Driver;
using Stubbl.Api.Authentication;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Events.TeamInvitationCreated.Version1;
using Stubbl.Api.Notifications.Email.TeamInvitation.Version1;
using Stubbl.Api.Services.EmailSender;

namespace Stubbl.Api.EventHandlers.Notifications.Email
{
    public class TeamInvitationCreatedEventHandler : IEventHandler<TeamInvitationCreatedEvent>
    {
        private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
        private readonly IEmailSender _emailSender;
        private readonly IMongoCollection<Team> _teamsCollection;

        public TeamInvitationCreatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
            IEmailSender emailSender, IMongoCollection<Team> teamsCollection)
        {
            _authenticatedUserAccessor = authenticatedUserAccessor;
            _emailSender = emailSender;
            _teamsCollection = teamsCollection;
        }

        public async Task HandleAsync(TeamInvitationCreatedEvent @event, CancellationToken cancellationToken)
        {
            var team = await _teamsCollection.Find(t => t.Id == @event.TeamId)
                .SingleAsync(cancellationToken);

            var email = new TeamInvitationEmail
            (
                @event.EmailAddress,
                team.Name,
                _authenticatedUserAccessor.AuthenticatedUser.Name,
                @event.InvitationId
            );

            await _emailSender.SendEmailAsync(email, cancellationToken);
        }
    }
}