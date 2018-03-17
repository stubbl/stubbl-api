namespace Stubbl.Api.Core.Queries.CountTeamMembers.Version1
{
   using Gunnsoft.Cqs.Queries;
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