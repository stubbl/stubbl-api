namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.RemoveTeamMember.Version1;
   using CodeContrib.CommandHandlers;
   using Events.TeamMemberRemoved.Version1;
   using Data.Collections.Teams;
   using Data.Collections.Shared;
   using Exceptions.MemberCannotBeRemovedFromTeam.Version1;
   using Exceptions.MemberCannotManageMembers.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.MemberNotFound.Version1;
   using MongoDB.Driver;
   using Member = Data.Collections.Members.Member;

   public class RemoveTeamMemberCommandHandler : ICommandHandler<RemoveTeamMemberCommand, TeamMemberRemovedEvent>
   {
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public RemoveTeamMemberCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Member> membersCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _membersCollection = membersCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamMemberRemovedEvent> HandleAsync(RemoveTeamMemberCommand command, CancellationToken cancellationToken)
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

         if (!team.Role.Permissions.Contains(Permission.ManageMembers))
         {
            throw new MemberCannotManageMembersException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
               team.Id
            );
         }

         if (command.MemberId == _authenticatedUserAccessor.AuthenticatedUser.Id)
         {
            throw new MemberCannotBeRemovedFromTeamException
            (
               command.MemberId,
               team.Id
            );
         }

         if (!await _membersCollection.Find(m => m.Id == command.MemberId && m.Teams.Any(t => t.Id == team.Id)).AnyAsync(cancellationToken))
         {
            throw new MemberNotFoundException
            (
               command.MemberId,
               team.Id
            );
         }

         var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
         var update = Builders<Team>.Update.PullFilter(t => t.Members, m => m.Id == command.MemberId);

         await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new TeamMemberRemovedEvent
         (
            team.Id,
            command.MemberId
         );
      }
   }
}