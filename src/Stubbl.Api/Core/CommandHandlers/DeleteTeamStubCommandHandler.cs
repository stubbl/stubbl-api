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
      private readonly IAuthenticatedUserAccessor _authenticatedUserAccessor;
      private readonly IMongoCollection<Stub> _stubsCollection;

      public DeleteTeamStubCommandHandler(IAuthenticatedUserAccessor authenticatedUserAccessor,
         IMongoCollection<Stub> stubsCollection)
      {
         _authenticatedUserAccessor = authenticatedUserAccessor;
         _stubsCollection = stubsCollection;
      }

      public async Task<TeamStubDeletedEvent> HandleAsync(DeleteTeamStubCommand command, CancellationToken cancellationToken)
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

         if (!team.Role.Permissions.Contains(Permission.ManageStubs))
         {
            throw new MemberCannotManageStubsException
            (
               _authenticatedUserAccessor.AuthenticatedUser.Id,
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