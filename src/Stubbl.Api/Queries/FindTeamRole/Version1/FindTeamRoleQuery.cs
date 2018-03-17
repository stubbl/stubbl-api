using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeamRole.Version1
{
    public class FindTeamRoleQuery : IQuery<FindTeamRoleProjection>
    {
        public FindTeamRoleQuery(ObjectId teamId, ObjectId roleId)
        {
            TeamId = teamId;
            RoleId = roleId;
        }

        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}