namespace Stubbl.Api.Core.Queries.FindTeam.Version1
{
   using Common.Queries;
   using MongoDB.Bson;

   public class FindTeamQuery : IQuery<FindTeamProjection>
   {
      public FindTeamQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}