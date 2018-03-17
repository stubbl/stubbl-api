using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.ListTeamStubs.Version1
{
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