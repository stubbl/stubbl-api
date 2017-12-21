namespace Stubbl.Api.Core.CommandHandlers
{
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Authentication;
   using Commands.UpdateTeamStub.Version1;
   using Common.CommandHandlers;
   using Data.Collections.Shared;
   using Data.Collections.Stubs;
   using Events.TeamStubUpdated.Version1;
   using Exceptions.MemberCannotManageStubs.Version1;
   using Exceptions.MemberNotAddedToTeam.Version1;
   using Exceptions.StubNotFound.Version1;
   using MongoDB.Driver;

   public class UpdateTeamStubCommandHandler : ICommandHandler<UpdateTeamStubCommand, TeamStubUpdatedEvent>
   {
      private readonly IAuthenticatedMemberAccessor _authenticatedMemberAccessor;
      private readonly IMongoCollection<Stub> _stubsCollection;

      public UpdateTeamStubCommandHandler(IAuthenticatedMemberAccessor authenticatedMemberAccessor,
         IMongoCollection<Stub> stubsCollection)
      {
         _authenticatedMemberAccessor = authenticatedMemberAccessor;
         _stubsCollection = stubsCollection;
      }

      public async Task<TeamStubUpdatedEvent> HandleAsync(UpdateTeamStubCommand command, CancellationToken cancellationToken)
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

         if (!await _stubsCollection.Find(s => s.TeamId == team.Id && s.Id == command.StubId).AnyAsync(cancellationToken))
         {
            throw new StubNotFoundException
            (
               command.StubId,
               team.Id
            );
         }

         var filter = Builders<Stub>.Filter.Where(s => s.Id == command.StubId);
         var update = Builders<Stub>.Update.Set(s => s.Name, command.Name)
            .Set(s => s.Request.HttpMethod, command.Request.HttpMethod)
            .Set(s => s.Request.Path, command.Request.Path)
            .Set(s => s.Request.QueryStringParameters, command.Request.QueryStringParameters?.Select(qcc => new QueryStringParameter {Key = qcc.Key, Value = qcc.Value}).ToList())
            .Set(s => s.Request.BodyTokens, command.Request.BodyTokens?.Select(bt => new BodyToken {Path = bt.Path, Type = bt.Type, Value = bt.Value}).ToList())
            .Set(s => s.Request.Headers, command.Request.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList())
            .Set(s => s.Response.HttpStatusCode, command.Response.HttpStatusCode)
            .Set(s => s.Response.Body, command.Response.Body)
            .Set(s => s.Response.Headers, command.Response.Headers?.Select(h => new Header {Key = h.Key, Value = h.Value}).ToList())
            .Set(s => s.Tags, command.Tags);

         await _stubsCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

         return new TeamStubUpdatedEvent
         (
            command.StubId,
            command.TeamId,
            command.Name,
            new Events.Shared.Version1.Request
            (
               command.Request.HttpMethod,
               command.Request.Path,
               command.Request.QueryStringParameters?.Select(qcc => new Events.Shared.Version1.QueryStringParameter(qcc.Key, qcc.Value)).ToList(),
               command.Request.BodyTokens?.Select(bt => new Events.Shared.Version1.BodyToken(bt.Path, bt.Type, bt.Value)).ToList(),
               command.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
            ),
            new Events.Shared.Version1.Response
            (
               command.Response.HttpStatusCode,
               command.Response.Body,
               command.Request.Headers?.Select(h => new Events.Shared.Version1.Header(h.Key, h.Value)).ToList()
            ),
            command.Tags
         );
      }
   }
}