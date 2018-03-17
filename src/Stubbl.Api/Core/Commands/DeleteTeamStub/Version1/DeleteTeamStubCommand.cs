namespace Stubbl.Api.Core.Commands.DeleteTeamStub.Version1
{
   using Gunnsoft.Cqs.Commands;
   using Events.TeamStubDeleted.Version1;
   using MongoDB.Bson;

   public class DeleteTeamStubCommand : ICommand<TeamStubDeletedEvent>
   {
      public DeleteTeamStubCommand(ObjectId teamId, ObjectId stubId)
      {
         TeamId = teamId;
         StubId = stubId;
      }

      public ObjectId StubId { get; }
      public ObjectId TeamId { get; }
   }
}