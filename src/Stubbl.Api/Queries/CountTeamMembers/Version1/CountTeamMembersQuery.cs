using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.CountTeamMembers.Version1
{
    public class CountTeamMembersQuery : IQuery<CountTeamMembersProjection>
    {
        public CountTeamMembersQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}