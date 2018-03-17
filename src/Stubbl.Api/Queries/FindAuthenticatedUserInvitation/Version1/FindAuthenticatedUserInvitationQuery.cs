using Gunnsoft.Cqs.Queries;
using MongoDB.Bson;

namespace Stubbl.Api.Queries.FindAuthenticatedUserInvitation.Version1
{
    public class FindAuthenticatedUserInvitationQuery : IQuery<FindAuthenticatedUserInvitationProjection>
    {
        public FindAuthenticatedUserInvitationQuery(ObjectId invitationId)
        {
            InvitationId = invitationId;
        }

        public ObjectId InvitationId { get; }
    }
}