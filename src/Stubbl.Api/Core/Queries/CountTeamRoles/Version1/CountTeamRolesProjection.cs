namespace Stubbl.Api.Core.Queries.CountTeamRoles.Version1
{
   using Gunnsoft.Cqs.Queries;

   public class CountTeamRolesProjection : IProjection
   {
      public CountTeamRolesProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}