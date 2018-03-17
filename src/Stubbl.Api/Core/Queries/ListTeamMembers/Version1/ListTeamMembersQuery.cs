namespace Stubbl.Api.Core.Queries.ListTeamMembers.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class ListTeamMembersQuery : IQuery<ListTeamMembersProjection>
   {
      public ListTeamMembersQuery(ObjectId teamId, int pageNumber, int pageSize)
      {
         TeamId = teamId;
         PageNumber = pageNumber;
         PageSize = pageSize;
      }

      public int PageNumber { get; }
      public int PageSize { get; }
      public ObjectId TeamId { get; }
   }
}