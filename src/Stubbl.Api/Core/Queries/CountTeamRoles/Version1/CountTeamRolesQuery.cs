namespace Stubbl.Api.Core.Queries.CountTeamRoles.Version1
{
   using CodeContrib.Queries;
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