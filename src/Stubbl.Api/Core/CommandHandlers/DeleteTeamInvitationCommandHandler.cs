namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.DeleteTeamInvitation.Version1;
   using Common.CommandHandlers;
   using Events.TeamInvitationDeleted.Version1;
   using Data.Collections.Invitations;
   using Data.Collections.Shared;
   using Exceptions.InvitationNotFound.Version1;
   using Exceptions.MemberCannotManageInvitations.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;

   public class DeleteTeamInvitationCommandHandler : ICommandHandler<DeleteTeamInvitationCommand, TeamInvitationDeletedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;

      public DeleteTeamInvitationCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Invitation> invitationsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _invitationsCollection = invitationsCollection;
      }

      public async Task<TeamInvitationDeletedEvent> HandleAsync(DeleteTeamInvitationCommand command, CancellationToken cancellationToken)
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

         var filter = Builders<Invitation>.Filter.Where(i => i.Id == command.InvitationId && i.Team.Id == team.Id);

         var result = await _invitationsCollection.DeleteOneAsync(filter, cancellationToken);

         if (result.DeletedCount == 0)
         {
            throw new InvitationNotFoundException
            (
               command.InvitationId,
               team.Id
            );
         }

         return new TeamInvitationDeletedEvent
         (
            team.Id,
            command.InvitationId
         );
      }
   }
}