namespace Stubbl.Api.Core.Queries.CountTeamMembers.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class CountTeamMembersQuery : IQuery<CountTeamMembersProjection>
   {
      public CountTeamMembersQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}