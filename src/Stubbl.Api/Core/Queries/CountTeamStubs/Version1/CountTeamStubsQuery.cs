namespace Stubbl.Api.Core.Queries.CountTeamStubs.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class CountTeamStubsQuery : IQuery<CountTeamStubsProjection>
   {
      public CountTeamStubsQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}