namespace Stubbl.Api.Core.Queries.FindTeamRole.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class FindTeamRoleQuery : IQuery<FindTeamRoleProjection>
    {
      public FindTeamRoleQuery(ObjectId teamId, ObjectId roleId)
      {
         TeamId = teamId;
         RoleId = roleId;
      }

      public ObjectId RoleId { get; }
      public ObjectId TeamId { get; }
   }
}
