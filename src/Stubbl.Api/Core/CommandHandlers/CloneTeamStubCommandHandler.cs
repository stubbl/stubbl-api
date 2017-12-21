namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.CloneTeamStub.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Shared;
   using Events.TeamStubCloned.Version1;
   using Exceptions.MemberCannotManageStubs.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.StubNotFound.Version1;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Stub = Data.Collections.Stubs.Stub;

   public class CloneTeamStubCommandHandler : ICommandHandler<CloneTeamStubCommand, TeamStubClonedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Stub> _stubsCollection;

      public CloneTeamStubCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Stub> stubsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _stubsCollection = stubsCollection;
      }

      public async Task<TeamStubClonedEvent> HandleAsync(CloneTeamStubCommand command, CancellationToken cancellationToken)
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

         var stub = await _stubsCollection.Find(s => s.TeamId == team.Id && s.Id == command.StubId)
            .SingleOrDefaultAsync(cancellationToken);

         if (stub == null)
         {
            throw new StubNotFoundException
            (
               command.StubId,
               team.Id
            );
         }

         var clonedStub = stub;
         clonedStub.Id = ObjectId.GenerateNewId();
         clonedStub.Name = command.Name;

         await _stubsCollection.InsertOneAsync(stub, cancellationToken: cancellationToken);

         return new TeamStubClonedEvent
         (
            clonedStub.Id,
            clonedStub.TeamId,
            clonedStub.Name,
            new Events.Shared.Version1.Request
            (
               clonedStub.Request.HttpMethod,
               clonedStub.Request.Path,
               clonedStub.Request.QueryStringParameters?.Select(qcc => new Events.Shared.Version1.QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
               clonedStub.Request.BodyTokens?.Select(bt => new Events.Shared.Version1.BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
               clonedStub.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
            ),
            new Events.Shared.Version1.Response
            (
               clonedStub.Response.HttpStatusCode,
               clonedStub.Response.Body,
               clonedStub.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
            ),
            clonedStub.Tags
         );
      }
   }
}