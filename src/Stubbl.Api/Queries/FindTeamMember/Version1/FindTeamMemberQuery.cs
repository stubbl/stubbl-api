using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeamMember.Version1
{
    public class FindTeamMemberQuery : IQuery<FindTeamMemberProjection>
    {
        public FindTeamMemberQuery(ObjectId teamId, ObjectId memberId)
        {
            TeamId = teamId;
            MemberId = memberId;
        }

        public ObjectId MemberId { get; }
        public ObjectId TeamId { get; }
    }
}