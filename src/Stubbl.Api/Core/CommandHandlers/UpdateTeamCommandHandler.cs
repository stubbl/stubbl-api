namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.UpdateTeam.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Shared;
   using Data.Collections.Teams;
   using Events.TeamUpdated.Version1;
   using Exceptions.MemberCannotManageTeams.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Member = Data.Collections.Members.Member;

   public class UpdateTeamCommandHandler : ICommandHandler<UpdateTeamCommand, TeamUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public UpdateTeamCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Member> membersCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _membersCollection = membersCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamUpdatedEvent> HandleAsync(UpdateTeamCommand command, CancellationToken cancellationToken)
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

         if (!team.Role.Permissions.Contains(Permission.ManageTeams))
         {
            throw new MemberCannotManageTeamsException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               team.Id
            );
         }

         var filter = Builders<Team>.Filter.Where(t => t.Id == team.Id);
         var update = Builders<Team>.Update.Set(t => t.Name, command.Name);

         await _teamsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new TeamUpdatedEvent
         (
            team.Id,
            command.Name
         );
      }
   }
}