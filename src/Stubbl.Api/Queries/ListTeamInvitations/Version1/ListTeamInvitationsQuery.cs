using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.ListTeamInvitations.Version1
{
    public class ListTeamInvitationsQuery : IQuery<ListTeamInvitationsProjection>
    {
        public ListTeamInvitationsQuery(ObjectId teamId)
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}