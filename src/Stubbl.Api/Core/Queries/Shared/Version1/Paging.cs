namespace Stubbl.Api.Core.Queries.Shared.Version1
{
   using System;

   public class Paging
   {
      public Paging(int pageNumber, int pageSize, long totalCount)
      {
         PageNumber = pageNumber;
         PageSize = pageSize;
         TotalCount = totalCount;
         PageCount = (long) Math.Ceiling((double) totalCount / pageSize);
      }

      public long PageCount { get; }
      public int PageNumber { get; }
      public int PageSize { get; }
      public long TotalCount { get; }
   }
}