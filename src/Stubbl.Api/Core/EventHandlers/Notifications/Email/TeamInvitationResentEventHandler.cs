﻿namespace Stubbl.Api.Core.EventHandlers.Notifications.Email
{
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.SendEmail.Version1;
   using Common.Commands;
   using Common.EventHandlers;
   using Core.Notifications.Email.TeamInvitation.Version1;
   using Data.Collections.Invitations;
   using Events.TeamInvitationResent.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class TeamInvitationResentEventHandler : IEventHandler<TeamInvitationResentEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly ICommandDispatcher _commandDispatcher;
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public TeamInvitationResentEventHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         ICommandDispatcher commandDispatcher, IMongoCollection<Invitation> invitationsCollection,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
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
            _authenticatedMemberAccessor.AuthenticatedMember.Name,
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