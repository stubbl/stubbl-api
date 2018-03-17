﻿namespace Stubbl.Api.Core.Queries.CountTeams.Version1
{
   using Gunnsoft.Cqs.Queries;

   public class CountTeamsProjection : IProjection
   {
      public CountTeamsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}