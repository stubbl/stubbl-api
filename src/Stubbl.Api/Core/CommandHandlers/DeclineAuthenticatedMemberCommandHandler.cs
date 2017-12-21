namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.DeclineAuthenticatedMemberInvitation.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Invitations;
   using Events.AuthenticatedMemberInvitationDeclined.Version1;
   using Exceptions.InvitationAlreadyUsed.Version1;
   using Exceptions.MemberAlreadyAddedToTeam.Version1;
   using Exceptions.MemberNotInvitedToTeam.Version1;
   using MongoDB.Driver;

   public class DeclineAuthenticatedMemberCommandHandler : ICommandHandler<DeclineAuthenticatedMemberInvitationCommand, AuthenticatedMemberInvitationDeclinedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public DeclineAuthenticatedMemberCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<AuthenticatedMemberInvitationDeclinedEvent> HandleAsync(DeclineAuthenticatedMemberInvitationCommand command, CancellationToken cancellationToken)
      {
         var invitation = await _invitationsCollection.Find(i => i.Id == command.InvitationId && i.EmailAddress.ToLower() == _authenticatedMemberAccessor.AuthenticatedMember.EmailAddress.ToLower())
            .SingleOrDefaultAsync(cancellationToken);

         if (invitation == null)
         {
            throw new MemberNotInvitedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               command.InvitationId
            );
         }

         if (invitation.Status != InvitationStatus.Sent)
         {
            throw new InvitationAlreadyUsedException
            (
               invitation.Id,
               invitation.Team.Id
            );
         }

         if (_authenticatedMemberAccessor.AuthenticatedMember.Teams.Any(t => t.Id == invitation.Team.Id))
         {
            throw new MemberAlreadyAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               invitation.Team.Id
            );
         }

         var filter = Builders<Invitation>.Filter.Where(i => i.Id == invitation.Id);
         var update = Builders<Invitation>.Update.Set(i => i.Status, InvitationStatus.Declined);

         await _invitationsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new AuthenticatedMemberInvitationDeclinedEvent
         (
            invitation.Id
         );
      }
   }
}