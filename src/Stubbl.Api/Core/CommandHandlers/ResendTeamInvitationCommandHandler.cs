namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.ResendTeamInvitation.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Invitations;
   using Data.Collections.Shared;
   using Events.TeamInvitationResent.Version1;
   using Exceptions.InvitationAlreadyUsed.Version1;
   using Exceptions.InvitationNotFound.Version1;
   using Exceptions.MemberCannotManageInvitations.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;

   public class ResendTeamInvitationCommandHandler : ICommandHandler<ResendTeamInvitationCommand, TeamInvitationResentEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public ResendTeamInvitationCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<TeamInvitationResentEvent> HandleAsync(ResendTeamInvitationCommand command, CancellationToken cancellationToken)
      {
         var team = _authenticatedMemberAccessor.AuthenticatedMember.Teams.SingleOrDefault(t => t.Id == command.TeamId);

         if (team == null)
         {
            throw new MemberNotAddedToTeamException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               command.TeamId
            );
         }

         if (!team.Role.Permissions.Contains(Permission.ManageInvitations))
         {
            throw new MemberCannotManageInvitationsException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               team.Id
            );
         }

         var invitation = await _invitationsCollection.Find(i => i.Team.Id == team.Id && i.Id == command.InvitationId)
            .SingleOrDefaultAsync(cancellationToken);

         if (invitation == null)
         {
            throw new InvitationNotFoundException
            (
               command.InvitationId,
               team.Id
            );
         }

         if (invitation.Status != InvitationStatus.Sent)
         {
            throw new InvitationAlreadyUsedException
            (
               command.InvitationId,
               invitation.Team.Id
            );
         }

         return new TeamInvitationResentEvent
         (
            team.Id,
            command.InvitationId
         );
      }
   }
}