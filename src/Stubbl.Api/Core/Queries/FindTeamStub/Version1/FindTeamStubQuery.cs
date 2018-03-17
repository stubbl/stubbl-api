namespace Stubbl.Api.Core.Queries.FindTeamStub.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class FindTeamStubQuery : IQuery<FindTeamStubProjection>
   {
      public FindTeamStubQuery(ObjectId teamId, ObjectId stubId)
      {
         TeamId = teamId;
         StubId = stubId;
      }

      public ObjectId TeamId { get; }
      public ObjectId StubId { get; }
   }
}