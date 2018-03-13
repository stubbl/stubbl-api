namespace Stubbl.Api.Core.EventHandlers.Notifications.Email
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.SendEmail.Version1;
   using Common.Commands;
   using Common.EventHandlers;
   using Core.Notifications.Email.TeamInvitation.Version1;
   using Data.Collections.Teams;
   using Events.TeamInvitationCreated.Version1;
   using MongoDB.Driver;

   public class TeamInvitationCreatedEventHandler : IEventHandler<TeamInvitationCreatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly ICommandDispatcher _commandDispatcher;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamInvitationCreatedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         ICommandDispatcher commandDispatcher, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _commandDispatcher = commandDispatcher;
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

         var command = new SendEmailCommand
         (
            email
         );

         await _commandDispatcher.DispatchAsync(command, cancellationToken);
      }
   }
}