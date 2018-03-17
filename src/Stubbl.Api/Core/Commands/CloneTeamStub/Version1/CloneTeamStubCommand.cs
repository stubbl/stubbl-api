namespace Stubbl.Api.Core.Commands.CloneTeamStub.Version1
{
   using Gunnsoft.Cqs.Commands;
   using Events.TeamStubCloned.Version1;
   using MongoDB.Bson;

   public class CloneTeamStubCommand : ICommand<TeamStubClonedEvent>
   {
      public CloneTeamStubCommand(ObjectId teamId, ObjectId stubId, string name)
      {
         TeamId = teamId;
         StubId = stubId;
         Name = name;
      }

      public ObjectId StubId { get; }
      public string Name { get; }
      public ObjectId TeamId { get; }
   }
}