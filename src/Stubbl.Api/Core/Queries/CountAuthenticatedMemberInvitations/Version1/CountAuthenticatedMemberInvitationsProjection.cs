namespace Stubbl.Api.Core.Queries.CountAuthenticatedMemberInvitations.Version1
{
   using Common.Queries;

   public class CountAuthenticatedMemberInvitationsProjection : IProjection
   {
      public CountAuthenticatedMemberInvitationsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}