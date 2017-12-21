namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.DeleteTeam.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Shared;
   using Data.Collections.Teams;
   using Events.TeamDeleted.Version1;
   using Exceptions.MemberCannotManageTeams.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using MongoDB.Driver;
   using Log = Data.Collections.Logs.Log;
   using Member = Data.Collections.Members.Member;
   using Stub = Data.Collections.Stubs.Stub;

   public class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, TeamDeletedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Log> _logsCollection;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Stub> _stubsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public DeleteTeamCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Log> logsCollection, IMongoCollection<Member> membersCollection, 
         IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _logsCollection = logsCollection;
         _membersCollection = membersCollection;
         _stubsCollection = stubsCollection;
         _teamsCollection = teamsCollection;
      }

      public async Task<TeamDeletedEvent> HandleAsync(DeleteTeamCommand command, CancellationToken cancellationToken)
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

         await _teamsCollection.DeleteOneAsync(filter, cancellationToken);

         return new TeamDeletedEvent
         (
            team.Id
         );
      }
   }
}