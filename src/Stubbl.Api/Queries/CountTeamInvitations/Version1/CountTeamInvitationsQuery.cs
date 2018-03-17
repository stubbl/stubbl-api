using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.CountTeamInvitations.Version1
{
    public class CountTeamInvitationsQuery : IQuery<CountTeamInvitationsProjection>
    {
        public CountTeamInvitationsQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}