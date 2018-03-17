using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.ListTeamMembers.Version1
{
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