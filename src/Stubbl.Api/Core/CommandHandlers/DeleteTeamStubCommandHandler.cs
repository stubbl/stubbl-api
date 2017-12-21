namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.DeleteTeamStub.Version1;
   using Common.CommandHandlers;
   using Events.TeamStubDeleted.Version1;
   using Data.Collections.Shared;
   using Data.Collections.Stubs;
   using Exceptions.MemberCannotManageStubs.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.StubNotFound.Version1;
   using MongoDB.Driver;

   public class DeleteTeamStubCommandHandler : ICommandHandler<DeleteTeamStubCommand, TeamStubDeletedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Stub> _stubsCollection;

      public DeleteTeamStubCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Stub> stubsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _stubsCollection = stubsCollection;
      }

      public async Task<TeamStubDeletedEvent> HandleAsync(DeleteTeamStubCommand command, CancellationToken cancellationToken)
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

         if (!team.Role.Permissions.Contains(Permission.ManageStubs))
         {
            throw new MemberCannotManageStubsException
            (
               _authenticatedMemberAccessor.AuthenticatedMember.Id,
               team.Id
            );
         }

         var filter = Builders<Stub>.Filter.Where(t => t.Id == command.StubId);

         var result = await _stubsCollection.DeleteOneAsync(filter, cancellationToken);

         if (result.DeletedCount == 0)
         {
            throw new StubNotFoundException
            (
               command.StubId,
               team.Id
            );
         }

            return new TeamStubDeletedEvent
         (
            team.Id,
            command.StubId
         );
      }
   }
}