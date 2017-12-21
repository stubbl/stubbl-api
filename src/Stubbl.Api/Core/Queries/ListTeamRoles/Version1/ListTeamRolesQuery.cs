namespace Stubbl.Api.Core.Queries.ListTeamRoles.Version1
{
   using Common.Queries;
   using MongoDB.Bson;

   public class ListTeamRolesQuery : IQuery<ListTeamRolesProjection>
   {
      public ListTeamRolesQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}