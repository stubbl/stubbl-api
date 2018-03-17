namespace Gunnsoft.CloudflareApi
{
   public class Paging
   {
      public Paging(int page, int perPage, int count, int totalCount)
      {
         PageNumber = page;
         PageSize = perPage;
         PageCount = count;
         TotalCount = totalCount;
      }

      public int PageCount { get; }
      public int PageSize { get; }
      public int PageNumber { get; }
      public int TotalCount { get; }
   }
}
