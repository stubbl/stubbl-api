using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindTeamInvitation.Version1
{
    public class FindTeamInvitationQuery : IQuery<FindTeamInvitationProjection>
    {
        public FindTeamInvitationQuery(ObjectId teamId, ObjectId invitationId)
        {
            TeamId = teamId;
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
        public ObjectId TeamId { get; }
    }
}