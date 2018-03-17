using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.ListTeamRoles.Version1
{
    public class ListTeamRolesQuery : IQuery<ListTeamRolesProjection>
    {
        public ListTeamRolesQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}