﻿namespace Stubbl.Api.Core.Queries.CountAuthenticatedUserInvitations.Version1
{
   using CodeContrib.Queries;

   public class CountAuthenticatedUserInvitationsProjection : IProjection
   {
      public CountAuthenticatedUserInvitationsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}