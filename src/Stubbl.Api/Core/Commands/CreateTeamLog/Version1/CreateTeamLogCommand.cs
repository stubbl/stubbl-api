namespace Stubbl.Api.Core.Commands.CreateTeamLog.Version1
{
   using System.Collections.Generic;
   using Gunnsoft.Cqs.Commands;
   using Events.TeamLogCreated.Version1;
   using MongoDB.Bson;

   public class CreateTeamLogCommand : ICommand<TeamLogCreatedEvent>
   {
      public CreateTeamLogCommand(ObjectId teamId, IReadOnlyCollection<ObjectId> stubIds, Request request, Response response)
      {
         TeamId = teamId;
         StubIds = stubIds;
         Request = request;
         Response = response;
      }

      public Request Request { get; }
      public Response Response { get; }
      public IReadOnlyCollection<ObjectId> StubIds { get; }
      public ObjectId TeamId { get; }
   }
}