namespace Stubbl.Api.Core.CommandHandlers
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.CreateTeamInvitation.Version1;
   using Gunnsoft.Cqs.CommandHandlers;
   using Data.Collections.Invitations;
   using Data.Collections.Members;
   using Data.Collections.Shared;
   using Events.TeamInvitationCreated.Version1;
   using Exceptions.MemberAlreadyAddedToTeam.Version1;
   using Exceptions.MemberAlreadyInvitedToTeam.Version1;
   using Exceptions.MemberCannotManageInvitations.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.RoleNotFound.Version1;
   using MongoDB.Driver;
   using Team = Data.Collections.Teams.Team;

   public class CreateTeamInvitationCommandHandler : ICommandHandler<CreateTeamInvitationCommand, TeamInvitationCreatedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Invitation> _invitationsCollection;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public CreateTeamInvitationCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Invitation> invitationsCollection, IMongoCollection<Member> membersCollection,
         IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _invitationsCollection = invitationsCollection;
         _membersCollection = membersCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamInvitationCreatedEvent> HandleAsync(CreateTeamInvitationCommand command, CancellationToken cancellationToken)
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

         if (string.Equals(_authenticatedUserAccessor.AuthenticatedUser.EmailAddress, command.EmailAddress, StringComparison.OrdinalIgnoreCase))
         {
            throw new MemberAlreadyAddedToTeamException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               team.Id
            );
         }

         if (!team.Role.Permissions.Contains(Permission.ManageInvitations))
         {
            throw new MemberCannotManageInvitationsException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               team.Id
            );
         }

         var member = await _membersCollection.Find(m => m.EmailAddress.ToLower() == command.EmailAddress.ToLower())
            .SingleOrDefaultAsync(cancellationToken);

         if (member != null && member.Teams.Any(t => t.Id == team.Id))
         {
            throw new MemberAlreadyAddedToTeamException
            (
               member.Id,
               team.Id
            );
         }

         if (await _invitationsCollection.Find(i => i.Team.Id == team.Id && i.EmailAddress.ToLower() == command.EmailAddress.ToLower() && i.Status == InvitationStatus.Sent).AnyAsync(cancellationToken))
         {
            throw new MemberAlreadyInvitedToTeamException
            (
               member.Id,
               team.Id
            );
         }

         var role = await _teamsCollection.Find(t => t.Id == team.Id && t.Roles.Any(r => r.Id == command.RoleId))
            .Project(t => t.Roles.SingleOrDefault(r => r.Id == command.RoleId))
            .SingleOrDefaultAsync(cancellationToken);

         if (role == null)
         {
            throw new RoleNotFoundException
            (
               command.RoleId,
               team.Id
            );
         }

         var invitation = new Invitation
         {
            Role = new Data.Collections.Invitations.Role
            {
               Id = role.Id,
               Name = role.Name
            },
            Team = new Data.Collections.Invitations.Team
            {
               Id = team.Id,
               Name = team.Name
            },
            EmailAddress = command.EmailAddress,
            Status = InvitationStatus.Sent
         };

         await _invitationsCollection.InsertOneAsync(invitation, cancellationToken: cancellationToken);

         return new TeamInvitationCreatedEvent
         (
            invitation.Id,
            invitation.Team.Id,
            invitation.Role.Id,
            invitation.EmailAddress
         );
      }
   }
}