namespace Stubbl.Api.Core.Queries.FindAuthenticatedMemberInvitation.Version1
{
   using Common.Queries;
   using MongoDB.Bson;

   public class FindAuthenticatedMemberInvitationQuery : IQuery<FindAuthenticatedMemberInvitationProjection>
   {
      public FindAuthenticatedMemberInvitationQuery(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}