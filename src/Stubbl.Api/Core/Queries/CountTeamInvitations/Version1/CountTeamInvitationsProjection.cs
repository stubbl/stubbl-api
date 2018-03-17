namespace Stubbl.Api.Core.Queries.CountTeamInvitations.Version1
{
   using Gunnsoft.Cqs.Queries;

   public class CountTeamInvitationsProjection : IProjection
   {
      public CountTeamInvitationsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}