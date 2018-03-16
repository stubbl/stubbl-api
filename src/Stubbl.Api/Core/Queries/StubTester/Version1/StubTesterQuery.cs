namespace Stubbl.Api.Core.Queries.StubTester.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class StubTesterQuery : IQuery<StubTesterProjection>
   {
      public StubTesterQuery(ObjectId teamId, Request request)
      {
         TeamId = teamId;
         Request = request;
      }

      public Request Request { get; }
      public ObjectId TeamId { get; }
   }
}