﻿namespace Stubbl.Api.Core.Queries.CountPermissions.Version1
{
   using Common.Queries;

   public class CountPermissionsProjection : IProjection
   {
      public CountPermissionsProjection(long totalCount)
      {
         TotalCount = totalCount;
      }

      public long TotalCount { get; }
   }
}