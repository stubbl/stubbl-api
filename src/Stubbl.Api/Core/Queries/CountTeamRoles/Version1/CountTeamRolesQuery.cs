namespace Stubbl.Api.Core.Queries.CountTeamRoles.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class CountTeamRolesQuery : IQuery<CountTeamRolesProjection>
   {
      public CountTeamRolesQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}