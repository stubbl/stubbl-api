namespace Stubbl.Api.Core.Queries.FindAuthenticatedUserInvitation.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class FindAuthenticatedUserInvitationQuery : IQuery<FindAuthenticatedUserInvitationProjection>
   {
      public FindAuthenticatedUserInvitationQuery(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}