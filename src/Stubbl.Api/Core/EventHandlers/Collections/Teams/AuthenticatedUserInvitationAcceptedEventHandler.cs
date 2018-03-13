﻿namespace Stubbl.Api.Core.EventHandlers.Collections.Teams
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Common.EventHandlers;
   using Data.Collections.Teams;
   using Events.AuthenticatedUserInvitationAccepted.Version1;
   using MongoDB.Driver;
   using Invitation = Data.Collections.Invitations.Invitation;
   using Role = Data.Collections.Teams.Role;

   public class AuthenticatedUserInvitationAcceptedEventHandler : IEventHandler<AuthenticatedUserInvitationAcceptedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public AuthenticatedUserInvitationAcceptedEventHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _invitationsCollection = invitationsCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task HandleAsync(AuthenticatedUserInvitationAcceptedEvent @event, CancellationToken cancellationToken)
      {
         var invitation = await _invitationsCollection.Find(i => i.Id == @event.InvitationId)
            .SingleAsync(cancellationToken);
         var role = await _teamsCollection.Find(t => t.Id == invitation.Team.Id)
            .Project(t => t.Roles.Single(r => r.Id == invitation.Role.Id))
            .SingleAsync(cancellationToken);

         var member = new Member
         {
            Id = _authenticatedUserAccessor.AuthenticatedUser.Id,
            Name = _authenticatedUserAccessor.AuthenticatedUser.Name,
            EmailAddress = _authenticatedUserAccessor.AuthenticatedUser.EmailAddress,
            Role = new Role
            {
               Id = role.Id,
               Name = role.Name,
               Permissions = role.Permissions
            }
         };

         var filter = Builders<Team>.Filter.Where(t => t.Id == invitation.Team.Id);
         var update = Builders<Team>.Update.Push(t => t.Members, member);

         await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
      }
   }
}