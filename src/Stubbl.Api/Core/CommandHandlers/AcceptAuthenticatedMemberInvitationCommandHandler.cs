namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.AcceptAuthenticatedMemberInvitation.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Invitations;
   using Events.AuthenticatedMemberInvitationAccepted.Version1;
   using Exceptions.InvitationAlreadyUsed.Version1;
   using Exceptions.MemberAlreadyAddedToTeam.Version1;
   using Exceptions.MemberNotInvitedToTeam.Version1;
   using Exceptions.RoleNotFound.Version1;
   using Exceptions.TeamNotFound.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class AcceptAuthenticatedMemberInvitationCommandHandler : ICommandHandler<AcceptAuthenticatedMemberInvitationCommand, AuthenticatedMemberInvitationAcceptedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public AcceptAuthenticatedMemberInvitationCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<AuthenticatedMemberInvitationAcceptedEvent> HandleAsync(AcceptAuthenticatedMemberInvitationCommand command, CancellationToken cancellationToken)
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

         var team = await _teamsCollection.Find(t => t.Id == invitation.Team.Id)
            .SingleOrDefaultAsync(cancellationToken);

         if (team == null)
         {
            throw new TeamNotFoundException
            (
               invitation.Team.Id
            );
         }

         if (!team.Roles.Any(r => r.Id == invitation.Role.Id))
         {
            throw new RoleNotFoundException
            (
               invitation.Role.Id,
               invitation.Team.Id
            );
         }

         var filter = Builders<Invitation>.Filter.Where(i => i.Id == command.InvitationId);
         var update = Builders<Invitation>.Update.Set(i => i.Status, InvitationStatus.Accepted);

         await _invitationsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new AuthenticatedMemberInvitationAcceptedEvent
         (
            invitation.Id
         );
      }
   }
}