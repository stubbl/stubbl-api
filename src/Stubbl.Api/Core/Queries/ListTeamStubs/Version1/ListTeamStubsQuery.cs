namespace Stubbl.Api.Core.Queries.ListTeamStubs.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class ListTeamStubsQuery : IQuery<ListTeamStubsProjection>
   {
      public ListTeamStubsQuery(ObjectId teamId, string search, int pageNumber, int pageSize)
      {
         TeamId = teamId;
         Search = search;
         PageNumber = pageNumber;
         PageSize = pageSize;
      }

      public int PageNumber { get; }
      public int PageSize { get; }
      public string Search { get; }
      public ObjectId TeamId { get; }
   }
}