﻿namespace Stubbl.Api.Core.Queries.CountTeamMembers.Version1
{
   using CodeContrib.Queries;

   public class CountTeamMembersProjection : IProjection
   {
      public CountTeamMembersProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}