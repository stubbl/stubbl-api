using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.CountTeamRoles.Version1
{
    public class CountTeamRolesQuery : IQuery<CountTeamRolesProjection>
    {
        public CountTeamRolesQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}